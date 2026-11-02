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
using TN.Transactions.Application.Transactions.Command.AddIncome;
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.FilterPaymentByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllPayments;
using TN.Transactions.Application.Transactions.Queries.GetPaymentById;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Transactions.Infrastructure.ServiceImpl
{
    public class PaymentsServices : IPaymentsServices
    {
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        private readonly IBillNumberGenerator _billNumberGenerator;
        public PaymentsServices(ISettingServices settingServices, IBillNumberGenerator billNumberGenerator,IGetUserScopedData getUserScopedData,IUnitOfWork unitOfWork,FiscalContext fiscalContext, IMapper mapper, ITokenService tokenService, IDateConvertHelper dateConvertHelper)
        {
            _getUserScopedData=getUserScopedData;
             _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
            _billNumberGenerator = billNumberGenerator;

        }

        public async Task<Result<AddPaymentsResponse>> Add(AddPaymentsCommand addPaymentsCommand)
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



                        string nepaliDate = addPaymentsCommand.transactionDate
                         ?? (await _dateConverterHelper.ConvertToNepali(DateTime.Today));

                        var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);



                        var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                        if (companySettings == null)
                        {
                            return Result<AddPaymentsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                        }
                        string paymentNumber = "";
                        if (companySettings.PaymentTransactionNumberType == TransactionNumberType.Manual)
                        {
                            paymentNumber = addPaymentsCommand.paymentsNumber!;
                        }
                        else
                        {
                            paymentNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "PAYMENT", fiscalYear.Data.fyName);
                        }



                        bool paymentsNumberExists = await _unitOfWork.BaseRepository<TransactionDetail>()
                        .AnyAsync(p => p.TransactionNumber == paymentNumber && p.CompanyId == companyId);



                        if (paymentsNumberExists)
                        {
                            return Result<AddPaymentsResponse>.Failure($"Payment number '{paymentNumber}' already exists for this company.");
                        }







                        #region Journal ENtries

                        string newJournalId = Guid.NewGuid().ToString();
                        var totalAmount = addPaymentsCommand.addTransactionItemsForPayments.Sum(x => x.amount);

                        var journalDetails = new List<JournalEntryDetails>();


                        foreach (var item in addPaymentsCommand.addTransactionItemsForPayments)
                        {
                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                               .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("ledger not found.");

                            var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("ledger group not found.");



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



                        if (addPaymentsCommand.paymentMethodId != null)
                        {

                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(addPaymentsCommand.paymentMethodId);

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
                                        0,
                                       addPaymentsCommand.totalAmount,

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
                                       addPaymentsCommand.totalAmount,

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
                                       addPaymentsCommand.totalAmount,

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
                                        addPaymentsCommand.totalAmount,
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
                                 "Payments Voucher",
                                DateTime.Now,
                                "Being Payments Vouchers recorded",
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
                                 addPaymentsCommand.totalAmount,
                                 addPaymentsCommand.narration,
                                 companyId,
                                userId,
                                DateTime.Now,
                                "",
                                 default,
                                 addPaymentsCommand.transactionMode,
                                 addPaymentsCommand.paymentMethodId,
                                    newJournalId,
                                    paymentNumber,
                                 addPaymentsCommand.addTransactionItemsForPayments?.Select(d =>
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

                        if (addPaymentsCommand.transactionMode != TransactionType.Payment)
                        {
                            return Result<AddPaymentsResponse>.Failure("This is not an Payment Transactions");
                        }

                       

                        #region Payment Added
                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                           .GetByGuIdAsync(addPaymentsCommand.paymentMethodId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Payment,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,
                                paymentMethodId: addPaymentsCommand.paymentMethodId,
                                chequeNumber: addPaymentsCommand.chequeNumber,
                                bankName: addPaymentsCommand.bankName,
                                accountName: addPaymentsCommand.accountName,
                                companyId: companyId,
                                chequeDate: DateTime.Now
                            );
                        }
                        else
                        {
                            payment = new PaymentsDetails(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Payment,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,
                                transactionDetailsId: newId,

                                paymentMethodId: addPaymentsCommand.paymentMethodId,
                                companyId

                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                        #endregion


          


                        await _unitOfWork.SaveChangesAsync();

                        scope.Complete();

                        var resultDTOs = _mapper.Map<AddPaymentsResponse>(transactionData);
                        return Result<AddPaymentsResponse>.Success(resultDTOs);
                    }
                    catch (Exception ex)
                    {
                        scope.Dispose();
                        throw new Exception("An error occurred while adding transaction", ex);
                    }
                }

            }
            catch(Exception ex)
            {
                return Result<AddPaymentsResponse>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _tokenService.GetUserId();


                var paymentDetailsList = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id && x.TransactionMode == TransactionDetail.TransactionType.Payment);

                var singlePaymentDetail = paymentDetailsList.FirstOrDefault();
                if (singlePaymentDetail is null)
                {
                    return Result<bool>.Failure("NotFound", "Payment cannot be found");
                }


                if (!string.IsNullOrEmpty(singlePaymentDetail.JournalEntriesId))
                {
                    var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(x => x.Id == singlePaymentDetail.JournalEntriesId,
                            query => query.Include(j => j.JournalEntryDetails)
                        );

                    var originalJournal = originalJournals.FirstOrDefault();
                    if (originalJournal is not null)
                    {
                        _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                    }
                }


                _unitOfWork.BaseRepository<TransactionDetail>().Delete(singlePaymentDetail);


                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting Payment having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllPaymentsQueryResposne>>> GetAll(PaginationRequest paginationRequest,string? ledgerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var paymentTransactions = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(
                        x => x.TransactionMode == TransactionType.Payment &&
                             (string.IsNullOrEmpty(ledgerId) || x.TransactionsItems.Any(ti => ti.LedgerId == ledgerId)),
                        query => query.Include(t => t.TransactionsItems)
                                      .Include(t => t.PaymentMethods) 
                    );

               
                var transactionIds = paymentTransactions.Select(t => t.Id).ToList();
                var chequePayments = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => transactionIds.Contains(x.TransactionDetailsId) &&
                                              x.TransactionType == TransactionType.Payment);

                // 🔹 Build response
                var paymentResponse = paymentTransactions.Select(t =>
                {
                    
                    var cheque = chequePayments.FirstOrDefault(c => c.TransactionDetailsId == t.Id);

                    return new GetAllPaymentsQueryResposne(
                        t.Id,
                        t.TransactionDate,
                        t.TotalAmount,
                        t.Narration,
                        t.TransactionMode,
                        cheque?.ChequeNumber ?? "",
                        cheque?.BankName ?? "",
                        t.TransactionsItems?.Select(detail => new AddTransactionItemsRequest(
                            detail.Id,
                            detail.Amount,
                            detail.Remarks,
                            detail.LedgerId
                        )).ToList() ?? new List<AddTransactionItemsRequest>()
                    );
                }).ToList();

                // 🔹 Pagination
                var totalItems = paymentResponse.Count;
                var paginatedIncome = paginationRequest != null && paginationRequest.IsPagination
                    ? paymentResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : paymentResponse;

                var pagedResult = new PagedResult<GetAllPaymentsQueryResposne>
                {
                    Items = paginatedIncome,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllPaymentsQueryResposne>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllPaymentsQueryResposne>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<GetPaymentByIdQueryResponse>> GetPaymentById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var paymentData = await _unitOfWork.BaseRepository<TransactionDetail>().
                    GetConditionalAsync(x => x.Id == id,
                    query => query.Include(rm => rm.TransactionsItems)
                    );


                var payments = await _unitOfWork.BaseRepository<ChequePayment>()
    .GetConditionalAsync(x => x.TransactionDetailsId == id
           && x.TransactionType == TransactionType.Payment);

                var paymentDetails = payments.FirstOrDefault();


             

                var td = paymentData.FirstOrDefault();

                var paymentDetailsData = await _unitOfWork.BaseRepository<PaymentsDetails>().FirstOrDefaultAsync(x => x.TransactionDetailsId == td.Id);


                var paymentDataResponse = new GetPaymentByIdQueryResponse
                (
                   td.Id,
                   td.TransactionDate,
                   td.TotalAmount,
                   td.Narration,
                   td.TransactionMode,
                   paymentDetailsData?.PaymentMethodId,
                   td.TransactionNumber,
                   paymentDetails?.ChequeNumber,
                   paymentDetails?.BankName,
                     paymentDetails?.AccountName,
                    td.TransactionsItems?.Select(detail => new UpdateTransactionItemRequest
                    (
                 
            
                           detail.Amount,
                            detail.Remarks,
                            detail.LedgerId

                    )).ToList() ?? new List<UpdateTransactionItemRequest>()
                );


                return Result<GetPaymentByIdQueryResponse>.Success(paymentDataResponse);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching payment by Id", ex);
            }
        }

        public async Task<Result<PagedResult<GetFilterPaymentQueryResponse>>> GetPaymentFilter(PaginationRequest paginationRequest, FilterPaymentDto filterPaymentDto)
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

                if (filterPaymentDto.startDate != default)
                    startEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterPaymentDto.startDate);

                if (filterPaymentDto.endDate != default)
                    endEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterPaymentDto.endDate);


                if (string.IsNullOrEmpty(filterPaymentDto.ledgerId) && startEnglishDate == null && endEnglishDate == null)
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
                var paymentResult = await _unitOfWork.BaseRepository<TransactionDetail>().GetConditionalAsync(
           x =>
               x.CreatedBy == userId &&
               x.TransactionMode == TransactionType.Payment &&
               (string.IsNullOrEmpty(filterPaymentDto.ledgerId) ||
                   x.TransactionsItems.Any(ti => ti.LedgerId == filterPaymentDto.ledgerId)) && 
               (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
               (endEnglishDate == null || x.CreatedAt <= endEnglishDate),
           q => q.Include(sd => sd.TransactionsItems)
       );

                var responseList = paymentResult.Select(i => new GetFilterPaymentQueryResponse(
                   i.Id,
                   i.TransactionDate,
                   i.TotalAmount,
                   i.Narration,
                   i.TransactionMode,
                   addTransactionItemsForPayments: i.TransactionsItems?.Select(detail => new AddTransactionItemsRequest(
                       detail.Id,
                          detail.Amount,
                          detail.Remarks,
                          detail.LedgerId
                   )).ToList() ?? new List<AddTransactionItemsRequest>()
                )).ToList();

                PagedResult<GetFilterPaymentQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterPaymentQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterPaymentQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }


                return Result<PagedResult<GetFilterPaymentQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching payment: {ex.Message}", ex);
            }
        }

        public async Task<Result<UpdatePaymentResponse>> Update(string id, UpdatePaymentCommand updatePaymentCommand)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Result<UpdatePaymentResponse>.Failure("InvalidRequest", "payment Id cannot be null or empty");
                }

                var paymentToBeUpdated = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id, q => q.Include(t => t.TransactionsItems));

                var payment = paymentToBeUpdated.FirstOrDefault();

                if (payment is null)
                {
                    return Result<UpdatePaymentResponse>.Failure("NotFound", "payment not found.");
                }


              

                _mapper.Map(updatePaymentCommand, payment);

                if (updatePaymentCommand.TransactionsItems != null)
                {

                    payment.TransactionsItems.Clear();


                    foreach (var itemDto in updatePaymentCommand.TransactionsItems)
                    {
                        var newItem = _mapper.Map<TransactionItems>(itemDto);
                        newItem.Id = Guid.NewGuid().ToString(); // new Id
                        newItem.TransactionDetailId = id;
                        newItem.Amount = itemDto.amount;
                        newItem.Remarks = itemDto.remarks;
                        newItem.LedgerId = itemDto.ledgerId;

                        payment.TransactionsItems.Add(newItem); // EF tracks automatically
                    }

                    await _unitOfWork.SaveChangesAsync();
                }


                await _unitOfWork.SaveChangesAsync();

                #region Update PaymentDetails

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                    .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Payment);

                paymentDetails.TotalAmount = updatePaymentCommand.totalAmount;
                if (paymentDetails is not null)
                {
                    _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                }

                #endregion

                //#region Delete PaymentDetails

                //var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                //    .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Payment);

                //if (paymentDetails is not null)
                //{
                //    _unitOfWork.BaseRepository<PaymentsDetails>().Delete(paymentDetails);
                //}

                //#endregion



                var oldJournal = await _unitOfWork.BaseRepository<JournalEntry>()
                        .FirstOrDefaultAsync(j => j.Id == payment.JournalEntriesId);

                if (oldJournal != null)
                {
                    //oldJournal.IsDeleted = true;
                    _unitOfWork.BaseRepository<JournalEntry>().Delete(oldJournal);
                    await _unitOfWork.SaveChangesAsync();
                }




                string newJournalId = Guid.NewGuid().ToString();
                var totalAmount = updatePaymentCommand.TransactionsItems.Sum(x => x.amount);

                var journalDetails = new List<JournalEntryDetails>();

                DateTime englishDate = await _dateConverterHelper.ConvertToEnglish(updatePaymentCommand.transactionDate);
                foreach (var item in updatePaymentCommand.TransactionsItems)
                {
                    var ledger = await _unitOfWork.BaseRepository<Ledger>()
                        .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("Ledger not found.");

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("Subledger group not found.");

                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        item.ledgerId,
                        item.amount, // Debit
                        0,           // Credit
                        englishDate,
                        _tokenService.CompanyId().FirstOrDefault(),
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));
                }


                if (updatePaymentCommand.paymentMethodId != null)
                {
                    var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                        .GetByGuIdAsync(updatePaymentCommand.paymentMethodId);

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId) ?? throw new Exception("ledger group not found.");

                    var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                        .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                    switch (subledgerGroup.Id)
                    {
                        case SubLedgerGroupConstants.CashInHands:
                        case SubLedgerGroupConstants.SundryDebtor:
                        case SubLedgerGroupConstants.SundryCreditors:
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                ledgers.Id,
                                0, // Debit
                                totalAmount,
                               englishDate,
                                 _tokenService.CompanyId().FirstOrDefault(),
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                            break;

                        default:
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                ledgers.Id,
                                0, // Debit
                                totalAmount,
                                englishDate,
                                _tokenService.CompanyId().FirstOrDefault(),
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                            break;
                    }
                }



                var journalData = new JournalEntry(
                        newJournalId,
                        "Payments Voucher",
                        englishDate,
                        "Being Payments Voucher updated",
                        _tokenService.GetUserId(),
                        _tokenService.CompanyId().FirstOrDefault(),
                        DateTime.UtcNow,
                        "",
                        default,
                        "",
                        _fiscalContext.CurrentFiscalYearId,
                        journalDetails,
                        true
                    );

                // 4. Validation
                if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                {
                    throw new InvalidOperationException("Journal entry is unbalanced.");
                }

                // 5. Save
                await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);
                await _unitOfWork.SaveChangesAsync();




                #region Payment Added
                PaymentsDetails payments;

                var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                   .GetByGuIdAsync(updatePaymentCommand.paymentMethodId);

                if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                {
                    payments = new ChequePayment(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Payment,
                        transactionDate: DateTime.Now,
                        totalAmount: totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updatePaymentCommand.paymentMethodId,
                        chequeNumber: updatePaymentCommand.chequeNumber,
                        bankName: updatePaymentCommand.bankName,
                        accountName: updatePaymentCommand.accountName,
                        companyId: payment.CompanyId,
                        chequeDate: DateTime.Now
                    );
                }
                else
                {
                    payments = new PaymentsDetails(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Payment,
                        transactionDate: DateTime.Now,
                        totalAmount: totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updatePaymentCommand.paymentMethodId,
                        companyId: payment.CompanyId
                    );
                }

                await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payments);

                #endregion



                scope.Complete();
                

                var resultResponse = new UpdatePaymentResponse(
                    updatePaymentCommand.transactionDate,
                    updatePaymentCommand.totalAmount,
                    updatePaymentCommand.narration,
                    updatePaymentCommand.transactionMode,
                   updatePaymentCommand.TransactionsItems.Select(d => new UpdateTransactionItemRequest(
       
                   
                       d.amount,
                       d.remarks,
                       d.ledgerId

                        )).ToList()
                );

                return Result<UpdatePaymentResponse>.Success(resultResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Payment.", ex);
            }
        }
    }
}
