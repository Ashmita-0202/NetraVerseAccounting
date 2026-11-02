using System.Linq;
using System.Transactions;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using TN.Account.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Transactions;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using TN.Transactions.Application.ServiceInterface;
using TN.Transactions.Application.Transactions.Command.AddTransactions;
using TN.Transactions.Application.Transactions.Command.UpdateTransactions;
using TN.Transactions.Application.Transactions.Queries.GetAllTransactions;
using TN.Transactions.Application.Transactions.Queries.GetTransactionsById;
using TN.Transactions.Application.Transactions.Queries.ReceiptVouchers;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Transactions.Infrastructure.ServiceImpl
{
    public class TransactionsService : ITransactionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly FiscalContext _fiscalContext;

        public TransactionsService(IUnitOfWork unitOfWork, IMapper mapper,FiscalContext fiscalContext, ITokenService tokenService, IDateConvertHelper dateConvertHelper)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _fiscalContext = fiscalContext;

        }

        public async Task<Result<AddTransactionsResponse>> Add(AddTransactionsCommand addTransactionsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;



                    DateTime entryDate = addTransactionsCommand.transactionDate == null
                      ? DateTime.Today
                      : await _dateConverterHelper.ConvertToEnglish(addTransactionsCommand.transactionDate);


                    string nepaliDate = await _dateConverterHelper.ConvertToNepali(entryDate);

                    #region Journal Entries

                    string newJournalId = Guid.NewGuid().ToString();



                    var journalDetails = new List<JournalEntryDetails>();


                    bool isIncome = addTransactionsCommand.transactionMode == TransactionType.Income;

                    bool isExpense = addTransactionsCommand.transactionMode == TransactionType.Expense;


                    foreach (var item in addTransactionsCommand.transactionsItemsCommand)
                    {
                        var incomeLedger = await _unitOfWork.BaseRepository<Ledger>()
                            .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("Income ledger not found.");
              
                           

                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(incomeLedger.SubLedgerGroupId) ?? throw new Exception("ledger group not found.");

                        if (isIncome)
                        {
                            if (subledgerGroup.Id != "623c3133-f4c7-41e5-a9c1-382c749d3a8a" &&
                                subledgerGroup.Id != "l2f3g4h5-a678-9012-l345-m678n901o234")
                            {
                                throw new Exception("Ledger is not part of an Income group.");
                            }
                        }
                        else if (isExpense)
                        {
                            if (subledgerGroup.Id != "8a6d5de6-5607-497e-8d7c-90d7494d7aa7" &&
                                subledgerGroup.Id != "i9c0d1e2-a345-6789-i012-j345k678l901")
                            {
                                throw new Exception("Ledger is not part of an Expenses group.");
                            }
                        }


                        //totalCredit += item.Amount;

                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            item.ledgerId,
                            debitAmount: isExpense ? item.amount : 0,
                            creditAmount: isIncome ? item.amount : 0,
                            entryDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }


                    if (addTransactionsCommand.paymentId != null)
                    {

                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(addTransactionsCommand.paymentId);

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
                                    addTransactionsCommand.totalAmount,
                                   0,

                                    entryDate,
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
                                    addTransactionsCommand.totalAmount,
                                   0,

                                    entryDate,
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
                                    addTransactionsCommand.totalAmount,
                                   0,

                                    entryDate,
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
                                    addTransactionsCommand.totalAmount,
                                    0,
                                    entryDate,
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
                     isIncome ? "Income Voucher" : "Expense Voucher",
                     entryDate,
                     isIncome ? "Being income recorded" : "Being expense recorded",
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



                    var transactionData = new TransactionDetail(
                          newId,
                          nepaliDate,
                          addTransactionsCommand.totalAmount,
                          addTransactionsCommand.narration,
                          companyId,
                          userId,
                          DateTime.UtcNow,
                          "",
                          default,
                          addTransactionsCommand.transactionMode,
                          addTransactionsCommand.paymentId,
                          "",
                          "",
                          addTransactionsCommand.transactionsItemsCommand?.Select(d =>
                          {
                              // Logic to decide debit or credit
                              decimal? debitAmount = null;
                              decimal? creditAmount = null;

                              if (addTransactionsCommand.transactionMode == TransactionType.Income)
                              {
                                  // Income → Credit the income ledger
                                  creditAmount = d.amount;
                              }
                              else
                              {
                                  // Expense or others → Debit the ledger
                                  debitAmount = d.amount;
                              }

                              return new TransactionItems(
                                  Guid.NewGuid().ToString(),
                                  d.amount,
                                   d.remarks,
                                     newId,
                                     d.ledgerId
                                  //debitAmount,
                                  //creditAmount                             
                              );
                          }).ToList() ?? new List<TransactionItems>()
                      );

                    await _unitOfWork.BaseRepository<TransactionDetail>().AddAsync(transactionData);





                    await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                    #endregion






                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddTransactionsResponse>(transactionData);
                    return Result<AddTransactionsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding transaction", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {

                var transactionsData = await _unitOfWork.BaseRepository<TransactionDetail>().GetByGuIdAsync(id);
                if (transactionsData is null)
                {
                    return Result<bool>.Failure("NotFound", "Transactions  Cannot be Found");
                }

                _unitOfWork.BaseRepository<TransactionDetail>().Delete(transactionsData);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Transactions having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllTransactionsByQueryResponse>>> GetAllTransactions(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(
                         null,
                         query => query.Include(t => t.TransactionsItems)
                    );

                var transactionsResponse = transactions.Select(t => new GetAllTransactionsByQueryResponse(
                    t.Id,
                    t.TransactionDate,
                    t.TotalAmount,
                    t.Narration,
                    t.TransactionMode,
                    t.PaymentMethodId,
                    t.TransactionsItems?.Select(detail => new ReceiptVouchersRequest(
                        detail.Amount,
                        detail.Remarks,
                        detail.LedgerId
                    )).ToList() ?? new List<ReceiptVouchersRequest>()
                )).ToList();

                var totalItems = transactionsResponse.Count;

                var paginatedTransactions = paginationRequest != null && paginationRequest.IsPagination
                    ? transactionsResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : transactionsResponse;

                var pagedResult = new PagedResult<GetAllTransactionsByQueryResponse>
                {
                    Items = paginatedTransactions,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllTransactionsByQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllTransactionsByQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<ReceiptVouchersByQueryResponse>> GetReceiptVouchers(string transactionsDetailsId, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactionsData = await _unitOfWork.BaseRepository<TransactionDetail>().
                    GetConditionalAsync(x => x.Id == transactionsDetailsId,
                    query => query.Include(rm => rm.TransactionsItems)
                    );

                //var td = transactionsData.FirstOrDefault(x=>x.Id == transactionsDetailsId);

                var transaction = transactionsData.FirstOrDefault();

                foreach (var item in transaction.TransactionsItems)
                {
                    var expenseLedger = await _unitOfWork.BaseRepository<Ledger>()
                        .GetByGuIdAsync(item.LedgerId) ?? throw new Exception("Expenses ledger not found.");

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(expenseLedger.SubLedgerGroupId) ?? throw new Exception("ledger group not found.");


                    if (subledgerGroup.Id != "h8f9c0d1-a234-5678-h901-i234j567k890" && subledgerGroup.Id != "i9c0d1e2-a345-6789-i012-j345k678l901")
                        throw new Exception("Ledger is not part of an Expenses group.");

                }



                var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                                      .GetByGuIdAsync(transaction.PaymentMethodId);

                var transactionDetailsResponse = new ReceiptVouchersByQueryResponse(
                           transaction.Id,
                           transaction.TransactionDate,
                           transaction.TotalAmount,
                           transaction.TransactionMode,
                           paymentMethod.SubLedgerGroupsId,
                           transaction.TransactionsItems?
                               .Where(detail => detail != null)
                               .Select(detail => new ReceiptVouchersRequest(
                                   detail.Amount,
                                   detail.Remarks ?? string.Empty,
                                   detail.LedgerId
                                   //detail.DebitAmount,
                                   //detail.CreditAmount

                               )).ToList() ?? new List<ReceiptVouchersRequest>()
                       );


                return Result<ReceiptVouchersByQueryResponse>.Success(transactionDetailsResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching transaction by Id", ex);
            }
        }

        public async Task<Result<GetTransactionsByIdQueryResponse>> GetTransactionsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactionsData = await _unitOfWork.BaseRepository<TransactionDetail>().
                    GetConditionalAsync(x => x.Id == id,
                    query => query.Include(rm => rm.TransactionsItems)
                    );

                var td = transactionsData.FirstOrDefault();
                var transactionDetailsResponse = new GetTransactionsByIdQueryResponse
                (
                   td.Id,
                   td.TransactionDate,
                   td.TotalAmount,
 
                   td.Narration,
                   td.TransactionMode,
                    td.TransactionsItems?.Select(detail => new ReceiptVouchersRequest
                    (
                           detail.Amount,
                            detail.Remarks,
                            detail.LedgerId
                            //detail.DebitAmount,
                            //detail.CreditAmount

                    )).ToList() ?? new List<ReceiptVouchersRequest>()
                );


                return Result<GetTransactionsByIdQueryResponse>.Success(transactionDetailsResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching transaction by Id", ex);
            }
        }

        public async Task<Result<UpdateTransactionsResponse>> Update(UpdateTransactionsCommand command, string id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                    {
                        return Result<UpdateTransactionsResponse>.Failure("InvalidRequest", "Transaction ID cannot be null or empty");
                    }

                    var transactions = await _unitOfWork.BaseRepository<TransactionDetail>()
                        .GetConditionalAsync(x => x.Id == id, q => q.Include(t => t.TransactionsItems));

                    var transaction = transactions.FirstOrDefault();
                    if (transaction == null)
                    {
                        return Result<UpdateTransactionsResponse>.Failure("NotFound", "Transaction not found.");
                    }

                    // Update main transaction fields
                    _mapper.Map(command, transaction);

                    // Update existing details
                    if (command.updateTransactionsDetails != null && command.updateTransactionsDetails.Any())
                    {
                        foreach (var detail in command.updateTransactionsDetails)
                        {
                            var existingDetail = transaction.TransactionsItems
                                .FirstOrDefault(d => d.TransactionDetailId == detail.transactionId);

                            if (existingDetail != null)
                            {
                                _mapper.Map(detail, existingDetail);

                                _unitOfWork.BaseRepository<TransactionItems>().Update(existingDetail);
                            }
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    // Build response
                    var response = new UpdateTransactionsResponse(
                        transaction.Id,
                        transaction.TransactionDate,
                        transaction.TotalAmount,
                        transaction.Narration,
                        transaction.TransactionsItems.Select(d => new UpdateTransactionDetailsRequest(
                            d.Amount,
                            d.Remarks,
                            d.LedgerId
                        )).ToList()
                    );

                    return Result<UpdateTransactionsResponse>.Success(response);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while updating the transaction.", ex);
                }
            }
        }
    }
}
    

