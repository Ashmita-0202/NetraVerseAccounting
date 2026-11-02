using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Transactions;
using TN.Account.Application.Account.Command.AddJournalEntry;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;
using TN.Account.Application.Account.Command.UpdateJournalEntry;
using TN.Account.Application.Account.Queries.FilterJournalByDate;
using TN.Account.Application.Account.Queries.FilterParties;
using TN.Account.Application.Account.Queries.JournalEntry;
using TN.Account.Application.Account.Queries.JournalEntryById;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Authentication.Domain.Entities.CompanySettings;


namespace TN.Account.Infrastructure.ServiceImpl
{
    public class JournalServices : IJournalServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly FiscalContext _fiscalContext;

        public JournalServices(IBillNumberGenerator billNumberGenerator,IDateConvertHelper dateConvertHelper,IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, FiscalContext fiscalContext, IFiscalYearService fiscalYearService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _dateConvertHelper = dateConvertHelper;
            _billNumberGenerator = billNumberGenerator;
            _fiscalYearService = fiscalYearService;
            _fiscalContext = fiscalContext;
        }
        public async Task<Result<AddJournalEntryResponse>> Add(AddJournalEntryCommand journalCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;


                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault()??"";

                    DateTime transactionDate = journalCommand.transactionDate == default
                    ? DateTime.Today
                   : await _dateConvertHelper.ConvertToEnglish(journalCommand.transactionDate);



                    var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x=>x.CompanyId==companyId);
                    if (companySettings == null)
                    {
                        return Result<AddJournalEntryResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }
                    string referenceNumber = "";
                    if (companySettings.JournalReference == JournalReferencesType.Manual)
                    {
                        referenceNumber = journalCommand.referenceNumber!;
                    }
                    else
                    {
                        referenceNumber = await _billNumberGenerator.GenerateJournalReference(companyId);
                    }

                    var journalData = new JournalEntry
                    (
                        newId,
                        referenceNumber,
                        transactionDate,
                        journalCommand.description,
                        userId,
                        companyId,
                        DateTime.Now,
                        "",
                        default,
                        "",
                        FyId,
                        journalCommand.AddJournalEntryDetails?.Select(d => new JournalEntryDetails
                          (
                              Guid.NewGuid().ToString(),
                              newId,
                              d.ledgerId,
                              d.debitAmount,
                              d.creditAmount,
                              transactionDate,
                              companyId,
                              _fiscalContext.CurrentFiscalYearId,
                              true
                          )
                          ).ToList() ?? new List<JournalEntryDetails>(),
                          true
                    );



                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                    await _unitOfWork.SaveChangesAsync();



                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddJournalEntryResponse>(journalData);
                    return Result<AddJournalEntryResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Complete();
                    throw new Exception("An error occurred while adding Journal", ex);
                }
            }
        }

        public async Task<Result<AddJournalEntryDetailsResponse>> AddJournalDetails(AddJournalEntryDetailsCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    var journalEntryDetailsData = new JournalEntryDetails
                    (

                        newId,
                        "DASDASD",
                        request.ledgerId,
                        request.debitAmount,
                        request.creditAmount,
                        default,
                        "",
                        _fiscalContext.CurrentFiscalYearId,
                        true

                    );

                    await _unitOfWork.BaseRepository<JournalEntryDetails>().AddAsync(journalEntryDetailsData);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddJournalEntryDetailsResponse>(journalEntryDetailsData);
                    return Result<AddJournalEntryDetailsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding JournalEntryDetails", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {


                var journal= await _unitOfWork.BaseRepository<JournalEntry>().GetByGuIdAsync(id);
                journal.IsActive = false;
                if (journal is null)
                {
                    return Result<bool>.Failure("NotFound", "Journal Cannot be Found");
                }

                _unitOfWork.BaseRepository<JournalEntry>().Update(journal);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting journal having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllJournalEntryByQueryResponse>>> GetAllJournalEntriesAsync(PaginationRequest paginationRequest)
        {
            try
            {

                var FyId = _fiscalContext.CurrentFiscalYearId;
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;

                // Apply filtering directly in the database query
                IEnumerable<JournalEntry> journalEntries;

                if (isSuperAdmin)
                {
                    journalEntries = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(
                            x =>x.IsActive && x.FyId == FyId,
                            query => query.Include(rm => rm.JournalEntryDetails));
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id));

                    journalEntries = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(
                            x => x.IsActive && x.FyId == FyId && companyIds.Contains(x.CompanyId),
                            query => query.Include(j => j.JournalEntryDetails));
                }
                else
                {
                    journalEntries = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(
                            x =>x.IsActive && x.FyId == FyId && x.CompanyId == companyId,
                            query => query.Include(j => j.JournalEntryDetails));
                }



                var journalEntryResponses = journalEntries
                       .OrderByDescending(journal => journal.TransactionDate) // Recent first
                       .Select(journal => new GetAllJournalEntryByQueryResponse(
                           journal.Id,
                           journal.ReferenceNumber,
                           journal.TransactionDate.ToString(),
                           journal.Description,
                           journal.CreatedBy,
                           journal.CreatedAt,
                           journal.CompanyId,
                           journal.JournalEntryDetails?.Select(detail => new JournalEntryDetailsDto(
                               detail.Id,
                               detail.LedgerId,
                               detail.DebitAmount,
                               detail.CreditAmount
                           )).ToList() ?? new List<JournalEntryDetailsDto>()
                       ))
                       .ToList();






                var totalItems = journalEntryResponses.Count;

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? journalEntryResponses
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : journalEntryResponses;

                var pagedResult = new PagedResult<GetAllJournalEntryByQueryResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllJournalEntryByQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllJournalEntryByQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<GetJournalEntryByIdResponse>> GetJournalById(string id, CancellationToken cancellationToken = default)
        {
            try
            {               

                var journalEntries = await _unitOfWork.BaseRepository<JournalEntry>().
                    GetConditionalAsync(x=>x.Id == id,
                    query => query.Include(rm => rm.JournalEntryDetails)
                    );

                var journal = journalEntries.FirstOrDefault();
                var journalEntryResponse = new GetJournalEntryByIdResponse(
                    journal.Id,
                    journal.ReferenceNumber,
                    (journal.TransactionDate).ToString(),
                    journal.Description,
                    journal.JournalEntryDetails?.Select(detail => new JournalEntryDetailsByIdDto(
                        detail.Id,
                        detail.LedgerId,
                        detail.DebitAmount,
                        detail.CreditAmount
                    )).ToList() ?? new List<JournalEntryDetailsByIdDto>()
                );

                return Result<GetJournalEntryByIdResponse>.Success(journalEntryResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching journal by using Id", ex);
            }
        }

        public async Task<Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>> GetJournalFilter(PaginationRequest paginationRequest,FilterJournalDTOs filterJournalDTOs)
        {
            try
            {
          
                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (filterJournalDTOs.startDate != default)
                    startEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterJournalDTOs.startDate);

                if (filterJournalDTOs.endDate != default)
                    endEnglishDate = await _dateConvertHelper.ConvertToEnglish(filterJournalDTOs.endDate);

            
                if (startEnglishDate == null && endEnglishDate == null)
                {
                    startEnglishDate = DateTime.Today;
                    endEnglishDate = DateTime.Today.AddDays(1).AddTicks(-1);
                }
                else if (startEnglishDate != null && endEnglishDate == null)
                {
                    endEnglishDate = startEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else if (endEnglishDate != null && startEnglishDate == null)
                {
                    startEnglishDate = endEnglishDate.Value.Date;
                }
                else if (endEnglishDate != null)
                {
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                var fiscalYearId = _fiscalContext.CurrentFiscalYearId;
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;

                IEnumerable<JournalEntry> entries;

                if (isSuperAdmin)
                {
                    entries = await _unitOfWork.BaseRepository<JournalEntry>().GetConditionalAsync(
                        x => x.IsActive &&
                            x.FyId == fiscalYearId &&
                            x.CreatedAt >= startEnglishDate &&
                            x.CreatedAt <= endEnglishDate &&
                            (string.IsNullOrEmpty(filterJournalDTOs.description) || x.Description.Contains(filterJournalDTOs.description)),
                        q => q.Include(x => x.JournalEntryDetails)
                    );
                }
                else if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(companyId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            q => q.Select(c => c.Id));

                    entries = await _unitOfWork.BaseRepository<JournalEntry>().GetConditionalAsync(
                        x => x.IsActive &&
                            x.FyId == fiscalYearId &&
                            companyIds.Contains(x.CompanyId) &&
                            x.CreatedAt >= startEnglishDate &&
                            x.CreatedAt <= endEnglishDate &&
                            (string.IsNullOrEmpty(filterJournalDTOs.description) || x.Description.Contains(filterJournalDTOs.description)),
                        q => q.Include(x => x.JournalEntryDetails)
                    );
                }
                else
                {
                    entries = await _unitOfWork.BaseRepository<JournalEntry>().GetConditionalAsync(
                        x => x.IsActive &&
                            x.FyId == fiscalYearId &&
                            x.CompanyId == companyId &&
                            x.CreatedAt >= startEnglishDate &&
                            x.CreatedAt <= endEnglishDate &&
                            (string.IsNullOrEmpty(filterJournalDTOs.description) || x.Description.Contains(filterJournalDTOs.description)),
                        q => q.Include(x => x.JournalEntryDetails)
                    );
                }

             
                var responseList = entries.Select(journal => new FilterJournalBySelectedDateQueryResponse(
                    journal.Id,
                    journal.ReferenceNumber,
                    journal.TransactionDate.ToString("yyyy-MM-dd"),
                    journal.Description,
                    journal.CreatedBy,
                    journal.CompanyId,
                    journal.CreatedAt,
                    journal.ModifiedBy,
                    journal.ModifiedAt,
                    journal.JournalEntryDetails?.Select(detail => new JournalEntryDetailsDto(
                        detail.Id,
                        detail.LedgerId,
                        detail.DebitAmount,
                        detail.CreditAmount
                    )).ToList() ?? new List<JournalEntryDetailsDto>()
                )).ToList();

                PagedResult<FilterJournalBySelectedDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterJournalBySelectedDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterJournalBySelectedDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching journal entries: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateJournalEntryResponse>> Update(string id, UpdateJournalEntryCommand updateJournalEntryCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateJournalEntryResponse>.Failure("NotFound", "Please provide valid journal Id");
                    }

                    var userId = _tokenService.GetUserId();
                    var journalEntries = await _unitOfWork.BaseRepository<JournalEntry>().
                    GetConditionalAsync(x => x.IsActive && x.Id == id,
                    query => query.Include(rm => rm.JournalEntryDetails)
                    );

                    var journal = journalEntries.FirstOrDefault();
                    if (journal == null)
                    {
                        return Result<UpdateJournalEntryResponse>.Failure("NotFound", "Journal entry not found.");
                    }

                    journal.CompanyId = _tokenService.CompanyId().FirstOrDefault();
                    journal.ModifiedBy = userId;
                    journal.ModifiedAt = DateTime.UtcNow;

                    if (updateJournalEntryCommand.journalEntries != null && updateJournalEntryCommand.journalEntries.Any())
                    {
                        foreach (var detail in updateJournalEntryCommand.journalEntries)
                        {
                            var existingJournal = await _unitOfWork.BaseRepository<JournalEntryDetails>().GetByGuIdAsync(detail.id);

                            if (existingJournal != null)
                            {
                                _mapper.Map(detail, existingJournal);
                                _unitOfWork.BaseRepository<JournalEntryDetails>().Update(existingJournal);
                            }
                            else
                            {
                                var newJournal = _mapper.Map<JournalEntryDetails>(detail);
                                newJournal.Id = Guid.NewGuid().ToString();
                                await _unitOfWork.BaseRepository<JournalEntryDetails>().AddAsync(newJournal);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync(); 
                    }



                    _mapper.Map(updateJournalEntryCommand, journal); 
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateJournalEntryResponse(
                             journal.Id,
                            journal.ReferenceNumber,
                            (journal.TransactionDate).ToString(),
                            journal.Description,
                             journal.JournalEntryDetails?.Select(detail => new UpdateJournalEntryDetails(
                                 detail.Id,
                                 detail.LedgerId,
                                 detail.DebitAmount,
                                 detail.CreditAmount
                             )).ToList() ?? new List<UpdateJournalEntryDetails>()
                         );
                  
                    return Result<UpdateJournalEntryResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the journal", ex);
                }

            }
        }
    }
}
