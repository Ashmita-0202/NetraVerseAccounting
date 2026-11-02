using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System.ComponentModel.Design;
using System.Linq;
using System.Transactions;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Application.Account.Command.ImportExcelForLedgers;
using TN.Account.Application.Account.Command.UpdateLedger;
using TN.Account.Application.Account.Queries.AccountPayable;
using TN.Account.Application.Account.Queries.AccountReceivable;
using TN.Account.Application.Account.Queries.ARAPByLedgerId;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Account.Application.Account.Queries.GetBalance;
using TN.Account.Application.Account.Queries.Ledger;
using TN.Account.Application.Account.Queries.LedgerBalanceDTOs;
using TN.Account.Application.Account.Queries.LedgerById;
using TN.Account.Application.Account.Queries.LedgerByLedgerGroupId;
using TN.Account.Application.Account.Queries.Parties;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;

namespace TN.Account.Infrastructure.ServiceImpl
{
    public class LedgerService : ILedgerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCacheRepository _memoryCacheRepository;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConverter;
        private readonly FiscalContext _fiscalContext;
        public LedgerService(IDateConvertHelper dateConverter,IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, ITokenService tokenService,IUnitOfWork unitOfWork,IMemoryCacheRepository memoryCacheRepository, IMapper mapper)
        {
            _dateConverter = dateConverter;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCacheRepository= memoryCacheRepository;
            _fiscalContext = fiscalContext;
        }

        public async Task<Result<AddLedgerResponse>> Add(AddLedgerCommand addLedgerCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {

                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    var companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";

                    var ledgerData = new Ledger(
                            newId,
                        addLedgerCommand.name,
                        DateTime.UtcNow,
                        addLedgerCommand.isInventoryAffected,
                        addLedgerCommand.address,
                        addLedgerCommand.panNo,
                        addLedgerCommand.phoneNumber,
                        addLedgerCommand.maxCreditPeriod,
                        addLedgerCommand.maxDuePeriod,
                        addLedgerCommand.subledgerGroupId,
                        companyId ?? "",
                        FyId,
                        addLedgerCommand.openingBalance,
                      
                        false,
                        true

                    );

                    await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledgerData);



                    decimal openingBalance = addLedgerCommand.openingBalance ?? 0;
                    string newJournalId = Guid.NewGuid().ToString();
                    var journalDetails = new List<JournalEntryDetails>();


                    #region Add Journal
                    if (openingBalance > 0)
                    {
                        // Positive balance → Debit newId, Credit ShareCapital
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            newId,                     // Debit side
                            openingBalance,
                            0,
                            DateTime.Now,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));

                        //journalDetails.Add(new JournalEntryDetails(
                        //    Guid.NewGuid().ToString(),
                        //    newJournalId,
                        //    LedgerConstants.ShareCapitalAccount, // Credit side
                        //    0,
                        //    openingBalance,
                        //    DateTime.Now,
                        //    companyId,
                        //    _fiscalContext.CurrentFiscalYearId
                        //));
                    }
                    else if (openingBalance < 0)
                    {
                        decimal absAmount = Math.Abs(openingBalance);

                        //journalDetails.Add(new JournalEntryDetails(
                        //    Guid.NewGuid().ToString(),
                        //    newJournalId,
                        //    LedgerConstants.ShareCapitalAccount, // Debit side
                        //    absAmount,
                        //    0,
                        //    DateTime.Now,
                        //    companyId,
                        //    _fiscalContext.CurrentFiscalYearId
                        //));

                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            newId,                     // Credit side
                            0,
                            absAmount,
                            DateTime.Now,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }



                    var journalData = new JournalEntry
                     (
                         newJournalId,
                         "Opening balance",
                         DateTime.Now,
                         "Being ledger has been Opened",
                         _tokenService.GetUserId(),
                         companyId,
                         DateTime.Now,
                         "",
                         default,
                          "",
                          FyId,
                         journalDetails,
                         true

                     );


                    //if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                    //{
                    //    throw new InvalidOperationException("Journal entry is unbalanced.");
                    //}
                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                    #endregion
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddLedgerResponse>(ledgerData);
                    return Result<AddLedgerResponse>.Success(resultDTOs);
                
                }catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding ledger ", ex);

                }
            }


        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var ledger = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(id);

                ledger.IsActive = false;
                if (ledger is null)
                {
                    return Result<bool>.Failure("NotFound", "Ledger Cannot be Found");
                }

                _unitOfWork.BaseRepository<Ledger>().Update(ledger);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting ledger having {id}", ex);
            }

        }

     

        public async Task<Result<PagedResult<GetAllLedgerByQueryResponse>>> GetAllLedger(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
             
                var (ledgers, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Ledger>();

           
                var queryable = await _unitOfWork.BaseRepository<Ledger>()
                    .FindBy(x => x.IsActive && (x.IsSeeded.HasValue && x.IsSeeded.Value) ||
                                 ((x.IsSeeded.HasValue && !x.IsSeeded.Value) && x.CompanyId == currentCompanyId));

             
                var finalQuery = queryable.AsNoTracking();

             
                var pagedResult = await finalQuery.ToPagedResultAsync(
                    paginationRequest.pageIndex,
                    paginationRequest.pageSize,
                    paginationRequest.IsPagination);

               
                var mappedItems = _mapper.Map<List<GetAllLedgerByQueryResponse>>(pagedResult.Data.Items);

                var response = new PagedResult<GetAllLedgerByQueryResponse>
                {
                    Items = mappedItems,
                    TotalItems = pagedResult.Data.TotalItems,
                    PageIndex = pagedResult.Data.PageIndex,
                    pageSize = pagedResult.Data.pageSize
                };

                return Result<PagedResult<GetAllLedgerByQueryResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all ledgers", ex);
            }
        }



        public async Task<Result<PagedResult<GetAllPartiesByQueriesResponse>>> GetAllParties(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Ledger>();

                IQueryable<Ledger> filteredParties;

                if (isSuperAdmin)
                {
                    filteredParties = ledger;
                        //.Where(x => x.FyId == fyId);
                }
                else
                {
                    filteredParties = ledger.Where(x => x.IsActive &&
                        //(x.FyId == fyId || x.FyId == "") &&
                        (x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "")
                    );

                    if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                    {
                        var companyIds = await _unitOfWork.BaseRepository<Company>()
                            .GetConditionalFilterType(
                                x => x.InstitutionId == institutionId,
                                query => query.Select(c => c.Id)
                            );

                        filteredParties = await _unitOfWork.BaseRepository<Ledger>()
                            .FindBy(x => x.IsActive && companyIds.Contains(x.CompanyId));
                           // && (x.FyId == fyId || x.FyId == ""));
                    }
                }

                var allowedSubLedgerGroupIds = new List<string>
                {
                    "3d5c1e24-d0ae-4f74-9c88-bf9f4b5c4d0b",
                    "7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72",
                    "dff66bb4-11e6-4e5f-8bb9-f00c01b90284",
                    "f5c2cba4-e4c7-496a-9f07-f2060c426e06"
                };

                filteredParties = filteredParties
                    .Where(x => allowedSubLedgerGroupIds.Contains(x.SubLedgerGroupId));

                // Paginate result
                var pagedResult = await filteredParties
                    .AsNoTracking()
                    .ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var response = _mapper.Map<PagedResult<GetAllPartiesByQueriesResponse>>(pagedResult.Data);

                return Result<PagedResult<GetAllPartiesByQueriesResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all ledger", ex);
            }
        }


        public async Task<Result<GetBalanceByQueryResponse>> GetBalance(string ledgerId)
        {
            try
            {
                // Get the current user and company
                var userId = _tokenService.GetUserId();
                var companyId = _tokenService.CompanyId().FirstOrDefault();

                if (string.IsNullOrEmpty(companyId))
                    return Result<GetBalanceByQueryResponse>.Failure("Company not found");

                // Fetch ledger balance grouped by LedgerId
                var balance = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalFilterType(
                        x => x.LedgerId == ledgerId && x.CompanyId == companyId,
                        query => query
                            .GroupBy(g => g.LedgerId)
                            .Select(g => new GetBalanceByQueryResponse(
                                g.Key,
                                g.Sum(x => x.DebitAmount - x.CreditAmount),
                                ""
                            ))
                    );

                var journalNet = balance.FirstOrDefault()?.balance ?? 0m;

                // Fetch ledger hierarchy
                var ledgerEntity = (await _unitOfWork.BaseRepository<Ledger>()
                    .GetConditionalAsync(
                        l =>l.IsActive && l.Id == ledgerId && (l.CompanyId == companyId || l.CompanyId == ""),
                        query => query
                            .Include(l => l.SubLedgerGroup)
                                .ThenInclude(slg => slg.LedgerGroup)
                                    .ThenInclude(lg => lg.Masters)
                    )).FirstOrDefault();

                if (ledgerEntity == null)
                    return Result<GetBalanceByQueryResponse>.Failure("Ledger not found");

                // Build hierarchy DTO
                var ledgerWithHierarchy = new LedgersDTOs(
                    ledgerEntity.Id,
                    ledgerEntity.Name,
                    new SubLedgerGroupDTOs(
                        ledgerEntity.SubLedgerGroup.Id,
                        ledgerEntity.SubLedgerGroup.Name,
                        new LedgerGroupDTOs(
                            ledgerEntity.SubLedgerGroup.LedgerGroup.Id,
                            ledgerEntity.SubLedgerGroup.LedgerGroup.Name,
                            new MasterDTOs(
                                ledgerEntity.SubLedgerGroup.LedgerGroup.MasterId,
                                ledgerEntity.SubLedgerGroup.LedgerGroup.Masters.Name
                            )
                        )
                    )
                );

                // Determine balance type based on master account
                string balanceType = ledgerEntity.SubLedgerGroup.LedgerGroup.Masters.Id switch
                {
                    MasterConstraints.Assets or MasterConstraints.Expenses => journalNet >= 0 ? "Dr" : "Cr",
                    MasterConstraints.Liabilities or MasterConstraints.Income => journalNet >= 0 ? "Cr" : "Dr",
                    _ => journalNet >= 0 ? "Dr" : "Cr"
                };

                decimal absoluteBalance = Math.Abs(journalNet);

                var result = new GetBalanceByQueryResponse(ledgerId, absoluteBalance, balanceType);
                return Result<GetBalanceByQueryResponse>.Success(result);
            }
            catch (Exception ex)
            {
                // Always include the original exception for debugging
                throw new Exception("An error occurred while getting balance.", ex);
            }
        }

        public async Task<Result<ARAPWithTotals>> GetAccountPayable(PaginationRequest paginationRequest, string? ledgerId = null)
        {
            try
            {

                #region Filter according to SubLedgerGroupId
                //var companyId = _tokenService.CompanyId().FirstOrDefault();

                //var journalEntries = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                //    .GetConditionalAsync(x => x.CompanyId == companyId);

                //string subledgerGroupId = "f5c2cba4-e4c7-496a-9f07-f2060c426e06";

                //var grouped = journalEntries
                //    .GroupBy(x => x.LedgerId)
                //    .Select(g => new
                //    {
                //        LedgerId = g.Key,
                //        Balance = g.Sum(x => x.CreditAmount - x.DebitAmount)
                //    }).ToList();
                //var accountPayableResult = new List<AccountPayableQueryResponse>();

                //foreach(var item in grouped)
                //{
                //    var ledger = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(item.LedgerId);
                //    if (ledger == null || ledger.SubLedgerGroupId != subledgerGroupId)
                //        continue;

                //    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                //.GetByGuIdAsync(ledger.SubLedgerGroupId);

                //    if (subledgerGroup == null) throw new Exception("Ledger group not found.");

                //    accountPayableResult.Add(new AccountPayableQueryResponse(
                //        item.LedgerId,
                //        item.Balance,
                //        subledgerGroup.Id
                //    ));
                //}

                //var totalItems = accountPayableResult.Count;

                //var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                //    ? accountPayableResult
                //        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                //        .Take(paginationRequest.pageSize)
                //        .ToList()
                //    : accountPayableResult;

                //var pagedResult = new PagedResult<AccountPayableQueryResponse>
                //{
                //    Items = paginatedJournalEntries,
                //    TotalItems = totalItems,
                //    PageIndex = paginationRequest?.pageIndex ?? 1,
                //    pageSize = paginationRequest?.pageSize ?? totalItems
                //};

                //return Result<PagedResult<AccountPayableQueryResponse>>.Success(pagedResult);

                #endregion
                var companyId = _tokenService.CompanyId().FirstOrDefault();


                var journalEntries = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalAsync(x => x.CompanyId == companyId &&
                 (x.Ledger.SubLedgerGroupId == SubLedgerGroupConstants.SundryDebtor ||
                  x.Ledger.SubLedgerGroupId == SubLedgerGroupConstants.SundryCreditors) &&
                     (string.IsNullOrEmpty(ledgerId) || x.LedgerId == ledgerId));

                if (!string.IsNullOrEmpty(ledgerId))
                {
                    journalEntries = journalEntries.Where(x => x.LedgerId == ledgerId).ToList();
                }

                var grouped = journalEntries
                    .GroupBy(x => x.LedgerId)
                    .Select(g => new
                    {
                        LedgerId = g.Key,
                        Balance = g.Sum(x => x.CreditAmount - x.DebitAmount),
                        BalanceType = g.Sum(x => x.CreditAmount - x.DebitAmount) >= 0 ? "Cr" : "Dr"
                    }).ToList();


                var accountPayableResult = new List<AccountPayableQueryResponse>();

                foreach (var item in grouped)
                {

                    if (item.Balance == 0.00m)
                        continue;


                    var ledger = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(item.LedgerId);
                    if (ledger == null)
                        continue;

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                .GetByGuIdAsync(ledger.SubLedgerGroupId);

                    if (subledgerGroup == null) throw new Exception("Ledger group not found.");

                    accountPayableResult.Add(new AccountPayableQueryResponse(
                        item.LedgerId,
                        Math.Abs(item.Balance),
                        item.BalanceType,
                        subledgerGroup.Id
                    ));
                }

                var totalAccountReceivable = grouped
                    .Where(r => r.BalanceType == "Dr")
                    .Sum(r => Math.Abs(r.Balance));

                var totalAccountPayable = grouped
                    .Where(r => r.BalanceType == "Cr")
                    .Sum(r =>Math.Abs(r.Balance));





                var totalItems = accountPayableResult.Count;

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? accountPayableResult
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : accountPayableResult;

                var pagedResult = new PagedResult<AccountPayableQueryResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };


                var response = new ARAPWithTotals(
                    PagedItems: pagedResult,
                    TotalReceivableAmount: totalAccountReceivable,
                    TotalPayableAmount: totalAccountPayable
                );

                return Result<ARAPWithTotals>.Success(response);
            }
            catch (Exception ex)
            {
                throw new Exception("An erro occurred while getting account payable");
            }
        }

        public async Task<Result<PagedResult<GetFilterLedgerByResponse>>> GetFilterLedger(PaginationRequest paginationRequest, FilterLedgerDto filterLedgerDto)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;

                var (ledgers, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<Ledger>();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                var filterLedgers = isSuperAdmin
                    ? ledgers
                    : ledgers.Where( x => x.IsActive && x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                // Handle Date Filters
                DateTime startEnglishDate = filterLedgerDto.startDate == default
                    ? DateTime.Today
                    : await _dateConverter.ConvertToEnglish(filterLedgerDto.startDate);

                DateTime endEnglishDate = filterLedgerDto.endDate == default
                    ? DateTime.Today
                    : await _dateConverter.ConvertToEnglish(filterLedgerDto.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

                var filteredResult = filterLedgers
                    .Where(x =>
                        (string.IsNullOrEmpty(filterLedgerDto.name) || x.Name.ToLower().Contains(filterLedgerDto.name.ToLower())) &&
                        (filterLedgerDto.startDate == default || x.CreatedDate >= startEnglishDate) &&
                        (filterLedgerDto.endDate == default || x.CreatedDate <= endEnglishDate)
                    )
                    .OrderBy(x => x.Name) // Alphabetical ordering
                    .ToList();


                var ledgerIds = filteredResult.Select(l => l.Id).ToList();
                var balances = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                     .GetConditionalFilterType(
                         x => ledgerIds.Contains(x.LedgerId) && (x.CompanyId == companyId || x.CompanyId == ""),
                         query => query
                             .AsNoTracking() // Read-only optimization
                             .GroupBy(g => g.LedgerId)
                             .Select(g => new
                             {
                                 LedgerId = g.Key,
                                 Balance = g.Sum(x => x.DebitAmount - x.CreditAmount)
                             })
                     );

                var balanceLookup = balances.ToDictionary(b => b.LedgerId, b => b.Balance);

                var ledgerEntities = await _unitOfWork.BaseRepository<Ledger>()
                       .GetConditionalAsync(
                           l => l.IsActive && ledgerIds.Contains(l.Id) && (l.CompanyId == companyId || l.CompanyId == ""),
                           query => query
                               .AsNoTracking()
                               .Include(l => l.SubLedgerGroup)
                                   .ThenInclude(slg => slg.LedgerGroup)
                                       .ThenInclude(lg => lg.Masters)
                       );

                var ledgerLookup = ledgerEntities.ToDictionary(l => l.Id, l => l);

                var responseList = filteredResult.Select(l =>
                {
                    var netBalance = balanceLookup.TryGetValue(l.Id, out var bal) ? bal : 0m;
                    var ledgerEntity = ledgerLookup.TryGetValue(l.Id, out var entity) ? entity : null;

                    // Determine balance type based on master ID
                    string balanceType = "Unknown";
                    if (ledgerEntity?.SubLedgerGroup?.LedgerGroup?.Masters != null)
                    {
                        var masterId = ledgerEntity.SubLedgerGroup.LedgerGroup.Masters.Id;
                        balanceType = masterId switch
                        {
                            MasterConstraints.Assets => netBalance >= 0 ? "Dr" : "Cr",
                            MasterConstraints.Expenses => netBalance >= 0 ? "Dr" : "Cr",
                            MasterConstraints.Liabilities => netBalance >= 0 ? "Cr" : "Dr",
                            MasterConstraints.Income => netBalance >= 0 ? "Cr" : "Dr",
                            _ => netBalance >= 0 ? "Dr" : "Cr"
                        };
                    }

                    return new GetFilterLedgerByResponse(
                        id: l.Id,
                        name: l.Name,
                        address: l.Address,
                        panNo: l.PanNo,
                        phoneNumber: l.PhoneNumber,
                        maxCreditPeriod: l.MaxCreditPeriod,
                        maxDuePeriod: l.MaxDuePeriod,
                        subledgerGroupId: l.SubLedgerGroupId,
                        isSeeded: l.IsSeeded,
                        balance: Math.Abs(netBalance),
                        balanceType: balanceType
                    );
                }).ToList();




                PagedResult<GetFilterLedgerByResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterLedgerByResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterLedgerByResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetFilterLedgerByResponse>>.Success(finalResponseList);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching ledgers: {ex.Message}", ex);
            }

        }






        public async Task<Result<PagedResult<GetFilterPartiesQueryResponse>>> GetFilterParties(PaginationRequest paginationRequest, FilterPartiesDto filterPartiesDto)
        {
            try
            {
                var fyId = _fiscalContext.CurrentFiscalYearId;

              
                var (ledgers, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Ledger>();

               
                var allowedLedgerGroupIds = new List<string>
        {
            "3d5c1e24-d0ae-4f74-9c88-bf9f4b5c4d0b", // Cash In Hand
            "7a9a6c6f-3b4a-4e58-b13c-c61e7bba9d72", // Bank Accounts
            "dff66bb4-11e6-4e5f-8bb9-f00c01b90284", // Sundry Debtors
            "f5c2cba4-e4c7-496a-9f07-f2060c426e06"  // Sundry Creditors
        };

               
                var filterLedgers = isSuperAdmin
                    ? ledgers
                    : ledgers.Where(x => x.IsActive && x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

               
                DateTime startEnglishDate = filterPartiesDto.startDate == default
                    ? DateTime.Today
                    : await _dateConverter.ConvertToEnglish(filterPartiesDto.startDate);

                DateTime endEnglishDate = filterPartiesDto.endDate == default
                    ? DateTime.Today
                    : await _dateConverter.ConvertToEnglish(filterPartiesDto.endDate);

                endEnglishDate = endEnglishDate.Date.AddDays(1).AddTicks(-1);

              
                var filteredResult = filterLedgers
                    .Where(x =>
                        allowedLedgerGroupIds.Contains(x.SubLedgerGroupId ?? string.Empty) && 
                        (string.IsNullOrEmpty(filterPartiesDto.name) || x.Name.ToLower().Contains(filterPartiesDto.name.ToLower())) &&
                        (filterPartiesDto.startDate == default || x.CreatedDate >= startEnglishDate) &&
                        (filterPartiesDto.endDate == default || x.CreatedDate <= endEnglishDate)
                    )
                    .OrderBy(x => x.Name) 
                    .ToList();

             
                var responseList = filteredResult.Select(p => new GetFilterPartiesQueryResponse(
                    p.Id,
                    p.Name,
                    p.CreatedDate,
                    p.IsInventoryAffected,
                    p.Address,
                    p.PanNo,
                    p.PhoneNumber,
                    p.MaxCreditPeriod,
                    p.MaxDuePeriod,
                    p.SubLedgerGroupId
                )).ToList();

                PagedResult<GetFilterPartiesQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterPartiesQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterPartiesQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetFilterPartiesQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching parties: {ex.Message}", ex);
            }

        }


        public async Task<Result<GetLedgerByIdQueryResponse>> GetLedgerById(string ledgerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var ledger = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(ledgerId);

                var ledgerResponse = _mapper.Map<GetLedgerByIdQueryResponse>(ledger);

                return Result<GetLedgerByIdQueryResponse>.Success(ledgerResponse);

            }
            catch (Exception ex) 
            { 
                throw new Exception("An error occurred while fetching Ledger by using Id", ex);
            }
        }

        public async Task<Result<List<GetAllLedgerByLedgerGroupIdResponse>>> GetLedgerByLedgerGroupId(string ledgerGroupId, CancellationToken cancellationToken = default)
        {
            var ledger = await _unitOfWork.BaseRepository<Ledger>().GetConditionalAsync(x => x.SubLedgerGroupId == ledgerGroupId);
            if (ledger is null)
            {
                return Result<List<GetAllLedgerByLedgerGroupIdResponse>>.Failure("NotFound", "LedgerGroup Data are not found");
            }

            var ledgerResponse = _mapper.Map<List<GetAllLedgerByLedgerGroupIdResponse>>(ledger);

            return Result<List<GetAllLedgerByLedgerGroupIdResponse>>.Success(ledgerResponse);
        }

        public async Task<Result<UpdateLedgerResponse>> Update(string ledgerId, UpdateLedgerCommand updateLedgerCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (ledgerId == null)
                    {
                        return Result<UpdateLedgerResponse>.Failure("NotFound", "Please provide valid ledgerId");
                    }

                    var ledgerToBeUpdated = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(ledgerId);
                    if (ledgerToBeUpdated is null)
                    {
                        return Result<UpdateLedgerResponse>.Failure("NotFound", "Ledger are not Found");
                    }
                    ledgerToBeUpdated.CreatedDate = DateTime.Now;
                    _mapper.Map(updateLedgerCommand, ledgerToBeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateLedgerResponse        
                        (
                       
                            ledgerToBeUpdated.Name,
                            ledgerToBeUpdated.IsInventoryAffected,
                            ledgerToBeUpdated.Address,
                            ledgerToBeUpdated.PanNo,
                            ledgerToBeUpdated.PhoneNumber,
                            ledgerToBeUpdated.MaxCreditPeriod,
                            ledgerToBeUpdated.MaxDuePeriod,
                            ledgerToBeUpdated.SubLedgerGroupId,
                            ledgerToBeUpdated.OpeningBalance
                
                        );

                    return Result<UpdateLedgerResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Ledger", ex);
                }

            }  
        }

        public async Task<Result<PagedResult<AccountReceivableQueryResponse>>> GetAccountReceivable(PaginationRequest paginationRequest,string? LedgerId)
        {
            try
            {
                var companyId = _tokenService.CompanyId().FirstOrDefault();

                var journalEntries = await _unitOfWork.BaseRepository<JournalEntryDetails>()
                    .GetConditionalAsync(x => x.CompanyId == companyId);

                string subledgerGroupId = "dff66bb4-11e6-4e5f-8bb9-f00c01b90284";
                if (!string.IsNullOrEmpty(LedgerId))
                {
                    journalEntries = journalEntries.Where(x => x.LedgerId == LedgerId).ToList();
                }
                var grouped = journalEntries
                    .GroupBy(x => x.LedgerId)
                    .Select(g => new
                    {
                        LedgerId = g.Key,
                        Balance = g.Sum(x => x.CreditAmount - x.DebitAmount)
                    }).ToList();
                var accountPayableResult = new List<AccountReceivableQueryResponse>();

                foreach (var item in grouped)
                {
                    var ledger = await _unitOfWork.BaseRepository<Ledger>().GetByGuIdAsync(item.LedgerId);
                    if (ledger == null || ledger.SubLedgerGroupId != subledgerGroupId)
                        continue;

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                .GetByGuIdAsync(ledger.SubLedgerGroupId);

                    if (subledgerGroup == null) throw new Exception("Ledger group not found.");

                    accountPayableResult.Add(new AccountReceivableQueryResponse(
                        item.LedgerId,
                        item.Balance,
                        subledgerGroup.Id
                    ));
                }

                var totalItems = accountPayableResult.Count;

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? accountPayableResult
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : accountPayableResult;

                var pagedResult = new PagedResult<AccountReceivableQueryResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<AccountReceivableQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                throw new Exception("An erro occurred while getting account payable");
            }
        }

        public async Task<Result<ArApByLedgerQueryResponse>> GetArApByLedger(string ledgerId, CancellationToken cancellationToken = default)
        {
            //var arApResponse = await GetAccountPayable( null , ledgerId);

            //if (arApResponse is not { IsSuccess: true, Data: { Items: { Count: > 0 } } items })
            //{
            //    return Result<ArApByLedgerQueryResponse>.Failure(
            //        arApResponse?.Message ?? $"Getting AR/AP by Ledger {ledgerId} failed."
            //    );
            //}

            //var item = arApResponse.Data.Ite.First();

            //var result = new ArApByLedgerQueryResponse
            //(item.ledgerId,
            //    item.balance,
            //    item.balanceType
            //    );


            return Result<ArApByLedgerQueryResponse>.Failure($"Error: {"Dasdsad"}");
        }

        // NormalizeName helper method
        private string NormalizeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            name = name.Trim().ToLower();
            name = name.Replace(" ", "");
            return name;
        }
        public async Task<Result<LedgerExcelResponse>> AddLedgerExcel(IFormFile formFile, CancellationToken cancellationToken = default)
        {
            Result<LedgerExcelResponse> result;

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var ledgers = new List<Ledger>();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? "";
                    string userId = _tokenService.GetUserId();

                    using var stream = new MemoryStream();
                    await formFile.CopyToAsync(stream, cancellationToken);
                    using var package = new ExcelPackage(stream);
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        result = Result<LedgerExcelResponse>.Failure("Worksheet not found");
                        return result;
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                 
                    Dictionary<string, int> headerMap = new();
                    int headerRow = 1; 


                    for (int col = 1; col <= colCount; col++)
                    {
                        var header = worksheet.Cells[headerRow, col].Text?.Trim().ToLower();
                        if (!string.IsNullOrWhiteSpace(header) && !headerMap.ContainsKey(header))
                        {
                            headerMap[header] = col;
                        }
                    }

                    for (int row = headerRow + 1; row <= rowCount; row++)
                    {
                        var name = worksheet.Cells[row, headerMap["name"]].Text?.Trim();
                        var subLedgerGroupName = worksheet.Cells[row, headerMap["subledgergroup"]].Text?.Trim();
                        var normalizedSubLedgerGroup = NormalizeName(subLedgerGroupName);

                        if (string.IsNullOrWhiteSpace(subLedgerGroupName))
                            throw new Exception($"SubLedgerGroup is missing at row {row}.");

                        
                        var subLedgerGroups = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetConditionalAsync(s => s.CompanyId == companyId);

                        var matchedSubLedgerGroup = subLedgerGroups
                            .FirstOrDefault(s => NormalizeName(s.Name) == normalizedSubLedgerGroup);

                        string subLedgerGroupId;
                        if (matchedSubLedgerGroup != null)
                        {
                            subLedgerGroupId = matchedSubLedgerGroup.Id;
                        }
                        else
                        {
                     

                            subLedgerGroupId = Guid.NewGuid().ToString();
                            var newSubLedgerGroup = new SubLedgerGroup
                           (
                                Guid.NewGuid().ToString(),           
                                 subLedgerGroupName,                
                                 default,
                                 companyId,                    
                                userId,                      
                                DateTime.UtcNow,              
                                 userId,                     
                               DateTime.UtcNow,            
                                 false,
                                 true
                           );
                            await _unitOfWork.BaseRepository<SubLedgerGroup>().AddAsync(newSubLedgerGroup);
                            await _unitOfWork.SaveChangesAsync();
                        }

                        // Read optional fields
                        var isInventoryAffected = bool.TryParse(worksheet.Cells[row, headerMap["isinventoryaffected"]].Text, out var inv) ? inv : (bool?)null;
                        var address = worksheet.Cells[row, headerMap["address"]].Text?.Trim();
                        var panNo = worksheet.Cells[row, headerMap["panno"]].Text?.Trim();
                        var phoneNumber = worksheet.Cells[row, headerMap["phonenumber"]].Text?.Trim();
                        var maxCreditPeriod = worksheet.Cells[row, headerMap["maxcreditperiod"]].Text?.Trim();
                        var maxDuePeriod = worksheet.Cells[row, headerMap["maxdueperiod"]].Text?.Trim();
                        var fyId = worksheet.Cells[row, headerMap["fyid"]].Text?.Trim();
                        var openingBalance = decimal.TryParse(worksheet.Cells[row, headerMap["openingbalance"]].Text, out var obVal) ? obVal : (decimal?)null;

                        // Create Ledger
                        var ledger = new Ledger(
                            Guid.NewGuid().ToString(),
                            name,
                            DateTime.UtcNow,
                            isInventoryAffected,
                            address,
                            panNo,
                            phoneNumber,
                            maxCreditPeriod,
                            maxDuePeriod,
                            subLedgerGroupId,
                            companyId,
                            fyId,
                            openingBalance,
                            false,
                            true
                        );

                        await _unitOfWork.BaseRepository<Ledger>().AddAsync(ledger);
                        ledgers.Add(ledger);
                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    result = Result<LedgerExcelResponse>.Success(new LedgerExcelResponse(ledgers.Count));
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred while adding Ledger: {ex.Message}", ex);
                }
            }

            return result;
        }
    }
    
}
