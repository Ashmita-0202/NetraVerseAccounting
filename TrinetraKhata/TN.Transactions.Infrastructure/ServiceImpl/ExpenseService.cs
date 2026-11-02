using AutoMapper;
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
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
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
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateExpense;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllExpense;
using TN.Transactions.Application.Transactions.Queries.GetExpenseById;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Infrastructure.ServiceImpl
{
    public class ExpenseService : IExpenseService
    {
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly FiscalContext _fiscalContext;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly ISettingServices _settingServices;

        public ExpenseService(ISettingServices settingServices,IGetUserScopedData getUserScopedData,IUnitOfWork unitOfWork,FiscalContext fiscalContext, IMapper mapper, ITokenService tokenService, IDateConvertHelper dateConvertHelper, IBillNumberGenerator billNumberGenerator)
        {
            _billNumberGenerator = billNumberGenerator;
            _getUserScopedData = getUserScopedData;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;

        }

        public async Task<Result<AddExpenseResponse>> AddExpense(AddExpenseCommand addExpenseCommand)
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
                            return Result<AddExpenseResponse>.Failure("Invalid CompanyId. Company does not exist.");
                        }
                        string expensesNumber = "";
                        if (companySettings.ExpensesTransactionNumberType == TransactionNumberType.Manual)
                        {
                            expensesNumber = addExpenseCommand.expensesNumber!;
                        }
                        else
                        {
                            expensesNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "EXPENSES", fiscalYear.Data.fyName);
                        }



                        bool expensesNumberExists = await _unitOfWork.BaseRepository<TransactionDetail>()
                        .AnyAsync(p => p.TransactionNumber == expensesNumber && p.CompanyId == companyId);

  

                        if (expensesNumberExists)
                        {
                            return Result<AddExpenseResponse>.Failure($"Bill number '{expensesNumber}' already exists for this company.");
                        }





                        string nepaliDate = addExpenseCommand.transactionDate
                            ?? (await _dateConverterHelper.ConvertToNepali(DateTime.Today));

                        #region Journal ENtries

                        string newJournalId = Guid.NewGuid().ToString();
                        var totalAmount = addExpenseCommand.addTransactionsItemsForExpense.Sum(x => x.amount);

                        var journalDetails = new List<JournalEntryDetails>();


                        foreach (var item in addExpenseCommand.addTransactionsItemsForExpense)
                        {
                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                               .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("ledger not found.");

                            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("Subledger group not found.");

                            if (subLedgerGroup.Id != "0423b6c3-51fa-44c4-bd9a-28fa9697ff55" &&
                                subLedgerGroup.Id != "34a841a6-339c-47d2-95d9-f3c9cb66c55a" &&
                                subLedgerGroup.Id != "8a6d5de6-5607-497e-8d7c-90d7494d7aa7")
                            {
                                throw new Exception("Ledger is not part of an Expenses group.");
                            }


                            journalDetails.Add(new JournalEntryDetails(
                               Guid.NewGuid().ToString(),
                               newJournalId,
                               item.ledgerId,
                            item.amount,
                               0,
                            DateTime.Now,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                           ));
                        }
                        if (addExpenseCommand.paymentMethodId != null)
                        {

                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(addExpenseCommand.paymentMethodId);

                            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                           .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId) ?? throw new Exception("Subledger group not found.");

                            var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subLedgerGroup.Id);



                            switch (subLedgerGroup.Id)
                            {
                                case SubLedgerGroupConstants.CashInHands:
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        ledgers.Id,
                                        0,
                                       addExpenseCommand.totalAmount,

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
                                        0,
                                       addExpenseCommand.totalAmount,

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
                                        0,
                                       addExpenseCommand.totalAmount,

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
                                        0,
                                        addExpenseCommand.totalAmount,
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
                                 "Expense Voucher",
                                DateTime.Now,
                                "Being Expense Vouchers recorded",
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
                                 addExpenseCommand.totalAmount,
                                 addExpenseCommand.narration,
                                 companyId,
                                userId,
                                DateTime.Now,
                                "",
                                 default,
                                 addExpenseCommand.transactionMode,
                                 addExpenseCommand.paymentMethodId,
                                    newJournalId,
                                    expensesNumber,
                                 addExpenseCommand.addTransactionsItemsForExpense?.Select(d =>
                                     new TransactionItems(
                                         Guid.NewGuid().ToString(),
                                         d.amount,
                                         d.remarks,
                                         newId,
                                         d.ledgerId
                                     )
                        ).ToList() ?? new List<TransactionItems>()
                             );


                        await _unitOfWork.BaseRepository<TransactionDetail>().AddAsync(transactionData);


                        if (addExpenseCommand.transactionMode != TransactionType.Expense)
                        {
                            return Result<AddExpenseResponse>.Failure("This is not an Expense Transactions");
                        }






                        #region Payment Added
                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                           .GetByGuIdAsync(addExpenseCommand.paymentMethodId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Expense,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,
                                paymentMethodId: addExpenseCommand.paymentMethodId,
                                chequeNumber: addExpenseCommand.chequeNumber,
                                bankName: addExpenseCommand.bankName,
                                accountName: addExpenseCommand.accountName,
                                chequeDate: DateTime.Now,
                                companyId: companyId
                            );
                        }
                        else
                        {
                            payment = new PaymentsDetails(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Expense,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,
                                paymentMethodId: addExpenseCommand.paymentMethodId,
                                companyId: companyId

                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                        #endregion


                        await _unitOfWork.SaveChangesAsync();

                        scope.Complete();

                        var resultDTOs = _mapper.Map<AddExpenseResponse>(transactionData);
                        return Result<AddExpenseResponse>.Success(resultDTOs);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while adding transaction", ex);
                    }
                }

            }
            catch (Exception ex)
            {
                return Result<AddExpenseResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _tokenService.GetUserId();

               
                var expenseDetailsList = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id && x.TransactionMode == TransactionDetail.TransactionType.Expense);

                var singleExpenseDetail = expenseDetailsList.FirstOrDefault();
                if (singleExpenseDetail is null)
                {
                    return Result<bool>.Failure("NotFound", "Expense cannot be found");
                }

              
                if (!string.IsNullOrEmpty(singleExpenseDetail.JournalEntriesId))
                {
                    var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(x => x.Id == singleExpenseDetail.JournalEntriesId,
                            query => query.Include(j => j.JournalEntryDetails)
                        );

                    var originalJournal = originalJournals.FirstOrDefault();
                    if (originalJournal is not null)
                    {
                        _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                    }
                }

                
                _unitOfWork.BaseRepository<TransactionDetail>().Delete(singleExpenseDetail);

            
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Expense having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllExpenseQueryResponse>>> GetAll(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var expenseTransactions = await _unitOfWork.BaseRepository<TransactionDetail>()
               .GetConditionalAsync(
                    x => x.TransactionMode == TransactionType.Expense,
                    query => query.Include(t => t.TransactionsItems)
               );

                var expenseResponse = expenseTransactions.Select(t => new GetAllExpenseQueryResponse(
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

                var totalItems = expenseResponse.Count;

                var paginatedIncome = paginationRequest != null && paginationRequest.IsPagination
                    ? expenseResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : expenseResponse;

                var pagedResult = new PagedResult<GetAllExpenseQueryResponse>
                {
                    Items = paginatedIncome,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllExpenseQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllExpenseQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }

        }

        public async Task<Result<GetExpenseByIdQueryResponse>> GetExpenseById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var expenseData = await _unitOfWork.BaseRepository<TransactionDetail>().
                    GetConditionalAsync(x => x.Id == id,
                    query => query.Include(rm => rm.TransactionsItems)
                    );


                var expensesPayment = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => x.TransactionDetailsId == id
                           && x.TransactionType == TransactionType.Expense);

                var expensesPaymentDetails = expensesPayment.FirstOrDefault();

                var td = expenseData.FirstOrDefault();

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>().FirstOrDefaultAsync(x => x.TransactionDetailsId == td.Id);



                var expenseDataResponse = new GetExpenseByIdQueryResponse
                (
                   td.Id,
                   td.TransactionDate,
                   td.TotalAmount,
                   td.Narration,
                   td.TransactionMode,
                   paymentDetails?.PaymentMethodId,
                   td?.TransactionNumber,

                     expensesPaymentDetails?.ChequeNumber,
                     expensesPaymentDetails?.BankName,
                        expensesPaymentDetails?.AccountName,
                    td.TransactionsItems?.Select(detail => new UpdateTransactionItemRequest
                    (
              
                           detail.Amount,
                            detail.Remarks,
                            detail.LedgerId

                    )).ToList() ?? new List<UpdateTransactionItemRequest>()
                );


                return Result<GetExpenseByIdQueryResponse>.Success(expenseDataResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching expense by Id", ex);
            }
        }

        public async Task<Result<PagedResult<GetFilterExpenseByDateQueryResponse>>> GetFilterExpense(PaginationRequest paginationRequest,FilterExpenseDto filterExpenseDto)
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

                if (filterExpenseDto.startDate != default)
                    startEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterExpenseDto.startDate);

                if (filterExpenseDto.endDate != default)
                    endEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterExpenseDto.endDate);


                if (string.IsNullOrEmpty(filterExpenseDto.narration) && startEnglishDate == null && endEnglishDate == null)
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
                var expenseResult = await _unitOfWork.BaseRepository<TransactionDetail>().GetConditionalAsync(
                x =>
                    x.CreatedBy == userId &&
                 x.TransactionMode == TransactionType.Expense &&
                (string.IsNullOrEmpty(filterExpenseDto.narration) || x.Narration.ToLower().Contains(filterExpenseDto.narration.ToLower())) &&

                        (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                        (endEnglishDate == null || x.CreatedAt <= endEnglishDate),
                 d => d.Include(t => t.TransactionsItems)
                       
                );

                var responseList = expenseResult.Select(r => new GetFilterExpenseByDateQueryResponse(
                   r.Id,
                   r.TransactionDate,
                   r.TotalAmount,
                   r.Narration,
                   r.TransactionMode,
                  addTransactionsItemsForExpense: r.TransactionsItems?.Select(detail => new AddTransactionItemsRequest(
                      detail.Id,
                          detail.Amount,
                          detail.Remarks,
                          detail.LedgerId
                     )).ToList() ?? new List<AddTransactionItemsRequest>()
                )).ToList();

                PagedResult<GetFilterExpenseByDateQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterExpenseByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterExpenseByDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetFilterExpenseByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Expense: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdateExpenseResponse>> UpdateExpense(string id, UpdateExpenseCommand updateExpenseCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        return Result<UpdateExpenseResponse>.Failure("InvalidRequest", "Expense ID cannot be null or empty");
                    }

                    var expenseToBeUpdated = await _unitOfWork.BaseRepository<TransactionDetail>()
                        .GetConditionalAsync(x => x.Id == id, q => q.Include(t => t.TransactionsItems));

                    var expense = expenseToBeUpdated.FirstOrDefault();

                    if (expense is null)
                    {
                        return Result<UpdateExpenseResponse>.Failure("NotFound", "Expense not found.");
                    }

                    // Map main expense properties
                    _mapper.Map(updateExpenseCommand, expense);

                    if (updateExpenseCommand.TransactionsItems != null)
                    {
                        // 1️⃣ Remove all existing children
                        expense.TransactionsItems.Clear();

                        // 2️⃣ Add all new children
                        foreach (var itemDto in updateExpenseCommand.TransactionsItems)
                        {
                            var newItem = _mapper.Map<TransactionItems>(itemDto);
                            newItem.Id = Guid.NewGuid().ToString(); // new Id
                            newItem.TransactionDetailId = id;
                            newItem.Amount = itemDto.amount;
                            newItem.Remarks = itemDto.remarks;
                            newItem.LedgerId = itemDto.ledgerId;

                            expense.TransactionsItems.Add(newItem); // EF tracks automatically
                        }

                        await _unitOfWork.SaveChangesAsync();
                    }




                    await _unitOfWork.SaveChangesAsync();


                        var oldJournal = await _unitOfWork.BaseRepository<JournalEntry>()
                                .FirstOrDefaultAsync(j => j.Id == expense.JournalEntriesId);

                        if (oldJournal != null)
                        {
                            //oldJournal.IsDeleted = true;
                            _unitOfWork.BaseRepository<JournalEntry>().Delete(oldJournal);
                            await _unitOfWork.SaveChangesAsync();
                        }

                    #region Update PaymentDetails

                    var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                        .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Expense);

                    paymentDetails.TotalAmount = updateExpenseCommand.totalAmount;
                    if (paymentDetails is not null)
                    {
                        _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                    }

                    #endregion


                    // Create new journal entry for updated expense
                    string newJournalId = Guid.NewGuid().ToString();
                        var journalDetails = new List<JournalEntryDetails>();

                        var totalAmount = updateExpenseCommand.TransactionsItems.Sum(x => x.amount);

                        DateTime englishDate = await _dateConverterHelper.ConvertToEnglish(updateExpenseCommand.transactionDate);

                        // 4a️⃣ Add expense items (Debit)
                        foreach (var item in updateExpenseCommand.TransactionsItems)
                        {
                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                                  .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("ledger not found.");

                            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("Subledger group not found.");

                            if (subLedgerGroup.Id != "0423b6c3-51fa-44c4-bd9a-28fa9697ff55" &&
                                subLedgerGroup.Id != "34a841a6-339c-47d2-95d9-f3c9cb66c55a" &&
                                subLedgerGroup.Id != "8a6d5de6-5607-497e-8d7c-90d7494d7aa7")
                            {
                                throw new Exception("Ledger is not part of an Expenses group.");
                            }

                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                item.ledgerId,
                                item.amount, // Debit
                                0,           // Credit
                                englishDate,
                                expense.CompanyId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

                        // 4b️⃣ Add payment (Credit)
                        if (updateExpenseCommand.paymentId != null)
                        {


                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                                .GetByGuIdAsync(updateExpenseCommand.paymentId);

                            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId) ?? throw new Exception("Subledger group not found");

                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subLedgerGroup.Id);

                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                ledger.Id,
                                0, // Debit
                                updateExpenseCommand.totalAmount, // Credit
                                englishDate,
                                expense.CompanyId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

                        var journalData = new JournalEntry(
                            newJournalId,
                            "Expense Voucher",
                            englishDate,
                            "Being Expense Vouchers updated",
                            expense.CreatedBy,
                            expense.CompanyId,
                            DateTime.UtcNow,
                            "",
                            default,
                            "",
                            _fiscalContext.CurrentFiscalYearId,
                            journalDetails,
                            true
                        );

                        if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is unbalanced.");

                        await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                        await _unitOfWork.SaveChangesAsync();



                        #region Payment Added
                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                           .GetByGuIdAsync(updateExpenseCommand.paymentId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Expense,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newJournalId,
                                paymentMethodId: updateExpenseCommand.paymentId,
                                chequeNumber: updateExpenseCommand.chequeNumber,
                                bankName: updateExpenseCommand.bankName,
                                accountName: updateExpenseCommand.accountName,
                                companyId: expense.CompanyId,
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
                                paymentMethodId: updateExpenseCommand.paymentId,
                                companyId: expense.CompanyId
                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                        #endregion





                        scope.Complete();

                        var resultResponse = new UpdateExpenseResponse(

                          id,
                         updateExpenseCommand.transactionDate,
                          updateExpenseCommand.totalAmount,
                            updateExpenseCommand.narration,
                            updateExpenseCommand.transactionMode,
                           updateExpenseCommand.TransactionsItems.Select(d => new UpdateTransactionItemRequest(
             
                               d.amount,
                               d.remarks,
                               d.ledgerId

                                )).ToList()
                        );

                        return Result<UpdateExpenseResponse>.Success(resultResponse);
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the Expense.", ex);
                }

                return Result<UpdateExpenseResponse>.Failure("UnexpectedError", "Expense update did not complete properly.");
            }
           

        
        }
    }
}
