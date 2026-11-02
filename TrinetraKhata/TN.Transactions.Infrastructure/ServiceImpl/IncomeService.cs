using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.AddExpense;
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllIncome;
using TN.Transactions.Application.Transactions.Queries.GetIncomeById;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Transactions.Infrastructure.ServiceImpl
{
    public class IncomeService : IIncomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        private readonly IBillNumberGenerator _billNumberGenerator;
        public IncomeService(IUnitOfWork unitOfWork,ISettingServices settingServices,IBillNumberGenerator billNumberGenerator,FiscalContext fiscalContext, IGetUserScopedData getUserScopedData,IMapper mapper, ITokenService tokenService, IDateConvertHelper dateConvertHelper)

        {
            _settingServices = settingServices;
            _unitOfWork = unitOfWork;
            _getUserScopedData = getUserScopedData;
            _mapper = mapper;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _billNumberGenerator = billNumberGenerator;

        }

        public async Task<Result<AddIncomeResponse>> Add(AddIncomeCommand addIncomeCommand)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        string newId = Guid.NewGuid().ToString();
                        var FyId = _fiscalContext.CurrentFiscalYearId;
                        string userId = _tokenService.GetUserId();
                        string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;

                        var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);



                        var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                        if (companySettings == null)
                        {
                            return Result<AddIncomeResponse>.Failure("Invalid CompanyId. Company does not exist.");
                        }
                        string incomeNumber = "";
                        if (companySettings.IncomeTransactionNumberType == TransactionNumberType.Manual)
                        {
                            incomeNumber = addIncomeCommand.incomeNumber!;
                        }
                        else
                        {
                            incomeNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "INCOME", fiscalYear.Data.fyName);
                        }



                        bool incomeNumberExists = await _unitOfWork.BaseRepository<TransactionDetail>()
                        .AnyAsync(p => p.TransactionNumber == incomeNumber && p.CompanyId == companyId);



                        if (incomeNumberExists)
                        {
                            return Result<AddIncomeResponse>.Failure($"Income number '{incomeNumber}' already exists for this company.");
                        }





                        string nepaliDate = addIncomeCommand.transactionDate
                         ?? (await _dateConverterHelper.ConvertToNepali(DateTime.Today));


            

                        #region Journal ENtries

                        string newJournalId = Guid.NewGuid().ToString();
                        var totalAmount = addIncomeCommand.addTransactionItemsForIncome.Sum(x => x.amount);

                        var journalDetails = new List<JournalEntryDetails>();


                        foreach (var item in addIncomeCommand.addTransactionItemsForIncome)
                        {
                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                               .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("ledger not found.");


                            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("subledger group not found.");

                            if (subLedgerGroup.Id != "623c3133-f4c7-41e5-a9c1-382c749d3a8a")


                            {
                                throw new Exception("Ledger is not part of an Income group.");
                            }


                            journalDetails.Add(new JournalEntryDetails(
                               Guid.NewGuid().ToString(),
                               newJournalId,
                               item.ledgerId,
                            0,
                               item.amount,
                            DateTime.Now,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                           ));
                        }
                        if (addIncomeCommand.paymentMethodId != null)
                        {

                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(addIncomeCommand.paymentMethodId);

                            var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                           .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId) ?? throw new Exception("ledger group not found.");

                            var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);



                            switch (subledgerGroup.Id)
                            {
                                case SubLedgerGroupConstants.CashInHands:
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgers.Id,
                                        addIncomeCommand.totalAmount,
                                       0,

                                        DateTime.Now,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                    break;

                                case SubLedgerGroupConstants.SundryDebtor:
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgers.Id,
                                        addIncomeCommand.totalAmount,
                                       0,

                                        DateTime.Now,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                    break;


                                case SubLedgerGroupConstants.SundryCreditors:
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgers.Id,
                                        addIncomeCommand.totalAmount,
                                       0,

                                        DateTime.Now,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                    break;

                                default:
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                    newJournalId,
                                        ledgers.Id,
                                        addIncomeCommand.totalAmount,
                                        0,
                                        DateTime.Now,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                    break;



                            }
                        }

                        var journalData = new JournalEntry
                            (
                                newJournalId,
                                 "Income Voucher",
                                DateTime.Now,
                                "Being Income Vouchers recorded",
                                userId,
                                companyId,
                                DateTime.UtcNow,
                                "",
                                default,
                                "",
                                FyId,
                                journalDetails,
                                true
                            );

                        if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                        {
                            throw new InvalidOperationException("Journal entry is unbalanced.");
                        }

                        await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                        #endregion


                     




                        var transactionData = new TransactionDetail(
                                 newId,
                                 nepaliDate,
                                 addIncomeCommand.totalAmount,
                                 addIncomeCommand.narration,
                                 companyId,
                                 userId,
                                 DateTime.UtcNow,
                                 "",
                                 default,
                                 addIncomeCommand.TransactionMode,
                                 addIncomeCommand.paymentMethodId,
                                    newJournalId,
                                    incomeNumber,
                                 addIncomeCommand.addTransactionItemsForIncome?.Select(d =>
                                     new TransactionItems(
                                         Guid.NewGuid().ToString() ,
                                         d.amount,
                                         d.remarks,
                                         newId,
                                         d.ledgerId
                                     )
                                 ).ToList() ?? new List<TransactionItems>()
                             );


                        await _unitOfWork.BaseRepository<TransactionDetail>().AddAsync(transactionData);


                        if (addIncomeCommand.TransactionMode != TransactionType.Income)
                        {
                            return Result<AddIncomeResponse>.Failure("This is not an Income Transactions");
                        }


                        #region Payment Added
                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                           .GetByGuIdAsync(addIncomeCommand.paymentMethodId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Income,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,
                                paymentMethodId: addIncomeCommand.paymentMethodId,
                                chequeNumber: addIncomeCommand.chequeNumber,
                                bankName: addIncomeCommand.bankName,
                                accountName: addIncomeCommand.accountName,
                                companyId: companyId,
                                chequeDate: DateTime.Now
                            );
                        }
                        else
                        {
                            payment = new PaymentsDetails(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Income,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,
                                paymentMethodId: addIncomeCommand.paymentMethodId,
                                companyId: companyId
                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                        #endregion





                        await _unitOfWork.SaveChangesAsync();

                        scope.Complete();

                        var resultDTOs = _mapper.Map<AddIncomeResponse>(transactionData);
                        return Result<AddIncomeResponse>.Success(resultDTOs);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while adding income", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                return Result<AddIncomeResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _tokenService.GetUserId();


                var incomeDetailsList = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id && x.TransactionMode == TransactionDetail.TransactionType.Income);

                var singleIncomeDetail = incomeDetailsList.FirstOrDefault();
                if (singleIncomeDetail is null)
                {
                    return Result<bool>.Failure("NotFound", "Income cannot be found");
                }


                if (!string.IsNullOrEmpty(singleIncomeDetail.JournalEntriesId))
                {
                    var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(x => x.Id == singleIncomeDetail.JournalEntriesId,
                            query => query.Include(j => j.JournalEntryDetails)
                        );

                    var originalJournal = originalJournals.FirstOrDefault();
                    if (originalJournal is not null)
                    {
                        _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                    }
                }


                _unitOfWork.BaseRepository<TransactionDetail>().Delete(singleIncomeDetail);


                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Income having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllIncomeQueryResponse>>> GetAll(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var incomeTransactions = await _unitOfWork.BaseRepository<TransactionDetail>()
               .GetConditionalAsync(
                    x => x.TransactionMode == TransactionType.Income,
                    query => query.Include(t => t.TransactionsItems)
               );

                var incomeResponse = incomeTransactions.Select(t => new GetAllIncomeQueryResponse(
                    t.Id,
                    t.TransactionDate,
                    t.TotalAmount,
                    t.Narration,
                    t.TransactionMode,
                    t.TransactionsItems?.Select(detail => new AddTransactionItemsRequest(
                         detail.Id,
                        detail.Amount,
                        detail.Remarks,
                        detail.LedgerId

                    )).ToList() ?? new List<AddTransactionItemsRequest>()
                )).ToList();

                var totalItems = incomeResponse.Count;

                var paginatedIncome = paginationRequest != null && paginationRequest.IsPagination
                    ? incomeResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : incomeResponse;

                var pagedResult = new PagedResult<GetAllIncomeQueryResponse>
                {
                    Items = paginatedIncome,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllIncomeQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllIncomeQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<GetIncomeByIdQueryResponse>> GetIncomeById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var incomeData = await _unitOfWork.BaseRepository<TransactionDetail>().
                    GetConditionalAsync(x => x.Id == id,
                    query => query.Include(rm => rm.TransactionsItems)
                    );

                var incomePayment = await _unitOfWork.BaseRepository<ChequePayment>()
                   .GetConditionalAsync(x => x.TransactionDetailsId == id
                          && x.TransactionType == TransactionType.Income);

                var incomePaymentDetails = incomePayment.FirstOrDefault();

                var td = incomeData.FirstOrDefault();

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>().FirstOrDefaultAsync(x => x.TransactionDetailsId == td.Id);



             
                var incomeDataResponse = new GetIncomeByIdQueryResponse
                (
                   td.Id,
                   td.TransactionDate,
                   td.TotalAmount,
                   td.Narration,
                   td.TransactionMode,
                   paymentDetails?.PaymentMethodId,
                   td.TransactionNumber,
                   incomePaymentDetails?.ChequeNumber,
                     incomePaymentDetails?.BankName,
                        incomePaymentDetails?.AccountName,
                    td.TransactionsItems?.Select(detail => new UpdateTransactionItemRequest
                    (
           
                  
                           detail.Amount,
                            detail.Remarks,
                            detail.LedgerId

                    )).ToList() ?? new List<UpdateTransactionItemRequest>()
                );


                return Result<GetIncomeByIdQueryResponse>.Success(incomeDataResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching income by Id", ex);
            }
        }

        public async Task<Result<PagedResult<GetFilterIncomeByDateQueryResponse>>> GetIncomeFilter(PaginationRequest paginationRequest,FilterIncomeDto filterIncomeDto)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<TransactionDetail>();

                var filterItems = isSuperAdmin ? ledger : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startEnglishDate = null;
                DateTime? endEnglishDate = null;

                if (filterIncomeDto.startDate != default)
                    startEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterIncomeDto.startDate);

                if (filterIncomeDto.endDate != default)
                    endEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterIncomeDto.endDate);


                if (string.IsNullOrEmpty(filterIncomeDto.narration) && startEnglishDate == null && endEnglishDate == null)
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
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }
                else if (startEnglishDate != null && endEnglishDate != null)
                {
                   
                    startEnglishDate = startEnglishDate.Value.Date;
                    endEnglishDate = endEnglishDate.Value.Date.AddDays(1).AddTicks(-1);
                }

                var userId = _tokenService.GetUserId();
                var incomeResult = await _unitOfWork.BaseRepository<TransactionDetail>().GetConditionalAsync(
                    x =>
                            x.CreatedBy == userId &&
                             x.TransactionMode == TransactionType.Income &&
                        (string.IsNullOrEmpty(filterIncomeDto.narration) || x.Narration.ToLower().Contains(filterIncomeDto.narration.ToLower())) &&

                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate),

                    q => q
                    .Include(sd => sd.TransactionsItems)
                );

                var responseList = incomeResult.Select(i => new GetFilterIncomeByDateQueryResponse(
                   i.Id,
                   i.TransactionDate,
                   i.TotalAmount,
                   i.Narration,
                   i.TransactionMode,
                   addTransactionItemsForIncome: i.TransactionsItems?.Select(detail => new AddTransactionItemsRequest(
                       detail.Id,
                          detail.Amount,
                          detail.Remarks,
                          detail.LedgerId
                   )).ToList() ?? new List<AddTransactionItemsRequest>()
                )).ToList();

                PagedResult<GetFilterIncomeByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterIncomeByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterIncomeByDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetFilterIncomeByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Income: {ex.Message}", ex);
            }

        }

        public async Task<Result<UpdateIncomeResponse>> Update(string id, UpdateIncomeCommand updateIncomeCommand)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Result<UpdateIncomeResponse>.Failure("InvalidRequest", "Income ID cannot be null or empty");
                }

                var incomeToBeUpdated = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id, q => q.Include(t => t.TransactionsItems));

                var income = incomeToBeUpdated.FirstOrDefault();

                if (income is null)
                {
                    return Result<UpdateIncomeResponse>.Failure("NotFound", "Income not found.");
                }

         
                _mapper.Map(updateIncomeCommand, income);



         



      

                if (updateIncomeCommand.TransactionsItems != null)
                {

                    income.TransactionsItems.Clear();

        
                    foreach (var itemDto in updateIncomeCommand.TransactionsItems)
                    {
                        var newItem = _mapper.Map<TransactionItems>(itemDto);
                        newItem.Id = Guid.NewGuid().ToString(); // new Id
                        newItem.TransactionDetailId = id;
                        newItem.Amount = itemDto.amount;
                        newItem.Remarks = itemDto.remarks;
                        newItem.LedgerId = itemDto.ledgerId;

                        income.TransactionsItems.Add(newItem); // EF tracks automatically
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
                await _unitOfWork.SaveChangesAsync();

                var oldJournal = await _unitOfWork.BaseRepository<JournalEntry>()
                        .FirstOrDefaultAsync(j => j.Id == income.JournalEntriesId);

                if (oldJournal != null)
                {
                    //oldJournal.IsDeleted = true;
                    _unitOfWork.BaseRepository<JournalEntry>().Delete(oldJournal);
                    await _unitOfWork.SaveChangesAsync();
                }

                #region Update PaymentDetails

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                    .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Income);

                paymentDetails.TotalAmount = updateIncomeCommand.totalAmount;
                if (paymentDetails is not null)
                {
                    _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                }

                #endregion



                DateTime englishDate = await _dateConverterHelper.ConvertToEnglish(updateIncomeCommand.transactionDate);


                string newJournalId = Guid.NewGuid().ToString();
                var totalAmount = updateIncomeCommand.TransactionsItems.Sum(x => x.amount);

                var journalDetails = new List<JournalEntryDetails>();

                foreach (var item in updateIncomeCommand.TransactionsItems)
                {
                    var ledger = await _unitOfWork.BaseRepository<Ledger>()
                              .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("ledger not found.");


                    var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("subledger group not found.");

                    if (subLedgerGroup.Id != "623c3133-f4c7-41e5-a9c1-382c749d3a8a")


                    {
                        throw new Exception("Ledger is not part of an Income group.");
                    }

                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        item.ledgerId,
                        0,          // Debit
                        item.amount, // Credit
                        englishDate,
                        income.CompanyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));
                }

                if (updateIncomeCommand.paymentId != null)
                {
                    var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                        .GetByGuIdAsync(updateIncomeCommand.paymentId);

                    var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId);

                    var ledger = await _unitOfWork.BaseRepository<Ledger>()
                        .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subLedgerGroup.Id);

                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        ledger.Id,
                        updateIncomeCommand.totalAmount, // Debit
                        0, // Credit
                        englishDate,
                        income.CompanyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));
                }

                if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                    throw new InvalidOperationException("Journal entry is unbalanced.");

                var journalData = new JournalEntry(
                    newJournalId,
                    "Income Voucher",
                    englishDate,
                    "Being Income Voucher updated",
                    income.CreatedBy,
                    income.CompanyId,
                    DateTime.UtcNow,
                    "",
                    default,
                    "",
                    _fiscalContext.CurrentFiscalYearId,
                    journalDetails,
                    true
                );

                await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                await _unitOfWork.SaveChangesAsync();




                #region Payment Added
                PaymentsDetails payment;

                var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                   .GetByGuIdAsync(updateIncomeCommand.paymentId);

                if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                {
                    payment = new ChequePayment(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Income,
                        transactionDate: DateTime.Now,
                        totalAmount: totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updateIncomeCommand.paymentId,
                        chequeNumber: updateIncomeCommand.chequeNumber,
                        bankName: updateIncomeCommand.bankName,
                        accountName: updateIncomeCommand.accountName,
                        companyId: income.CompanyId,
                        chequeDate: DateTime.Now
                    );
                }
                else
                {
                    payment = new PaymentsDetails(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Income,
                        transactionDate: DateTime.Now,
                        totalAmount: totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updateIncomeCommand.paymentId,
                        companyId: income.CompanyId
                    );
                }

                await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                #endregion



                scope.Complete();

                
                var resultResponse = new UpdateIncomeResponse(
                
                   updateIncomeCommand.transactionDate,
                   updateIncomeCommand.totalAmount,
                   updateIncomeCommand.narration,
                   updateIncomeCommand.transactionMode,
                   updateIncomeCommand.TransactionsItems.Select(d => new UpdateTransactionItemRequest(
          
                       d.amount,
                       d.remarks,
                       d.ledgerId

                        )).ToList()
                );

                return Result<UpdateIncomeResponse>.Success(resultResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Income.", ex);
            }
        }
    }
    
}
