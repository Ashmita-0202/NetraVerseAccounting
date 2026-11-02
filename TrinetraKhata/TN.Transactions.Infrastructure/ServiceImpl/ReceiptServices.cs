using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt;
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
using TN.Transactions.Application.Transactions.Command.AddPayments;
using TN.Transactions.Application.Transactions.Command.AddReceipt;
using TN.Transactions.Application.Transactions.Command.AddTransactionDetails;
using TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt;
using TN.Transactions.Application.Transactions.Command.UpdateIncome;
using TN.Transactions.Application.Transactions.Command.UpdatePayment;
using TN.Transactions.Application.Transactions.Command.UpdateReceipt;
using TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate;
using TN.Transactions.Application.Transactions.Queries.FilterReceiptByDate;
using TN.Transactions.Application.Transactions.Queries.GetAllReceipt;
using TN.Transactions.Application.Transactions.Queries.GetReceiptById;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Inventory.Domain.Entities.Inventories;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;



namespace TN.Transactions.Infrastructure.ServiceImpl
{
    public class ReceiptServices : IReceiptServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        private readonly IBillNumberGenerator _billNumberGenerator;


        public ReceiptServices(ISettingServices settingServices, IBillNumberGenerator billNumberGenerator,IUnitOfWork unitOfWork,IGetUserScopedData getUserScopedData,FiscalContext fiscalContext, IMapper mapper, ITokenService tokenService, IDateConvertHelper dateConvertHelper)
        {
            _unitOfWork = unitOfWork;
            _getUserScopedData = getUserScopedData;
            _mapper = mapper;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
            _billNumberGenerator = billNumberGenerator;

        }
        public async Task<Result<AddReceiptResponse>> Add(AddReceiptCommand addReceiptCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    string newId = Guid.NewGuid().ToString();
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;




                    string nepaliDate = addReceiptCommand.transactionDate
                     ?? (await _dateConverterHelper.ConvertToNepali(DateTime.Today));



                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);



                    var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                    if (companySettings == null)
                    {
                        return Result<AddReceiptResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }
                    string receiptNumber = "";
                    if (companySettings.ReceiptTransactionNumberType == TransactionNumberType.Manual)
                    {
                        receiptNumber = addReceiptCommand.receiptNumber!;
                    }
                    else
                    {
                        receiptNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "RECEIPT", fiscalYear.Data.fyName);
                    }



                    bool paymentsNumberExists = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .AnyAsync(p => p.TransactionNumber == receiptNumber && p.CompanyId == companyId);



                    if (paymentsNumberExists)
                    {
                        return Result<AddReceiptResponse>.Failure($"Receipts number '{receiptNumber}' already exists for this company.");
                    }



                    #region Journal ENtries

                    string newJournalId = Guid.NewGuid().ToString();
                    var totalAmount = addReceiptCommand.transactionItemsForReceipts.Sum(x => x.amount);

                    var journalDetails = new List<JournalEntryDetails>();


                    foreach (var item in addReceiptCommand.transactionItemsForReceipts)
                    {
                        var ledger = await _unitOfWork.BaseRepository<Ledger>()
                           .GetByGuIdAsync(item.ledgerId) ?? throw new Exception("Income ledger not found.");

                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(ledger.SubLedgerGroupId) ?? throw new Exception("ledger group not found.");


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
                    if (addReceiptCommand.paymentId != null)
                    {

                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(addReceiptCommand.paymentId);

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
                                    addReceiptCommand.totalAmount,
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
                                    addReceiptCommand.totalAmount,
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
                                    addReceiptCommand.totalAmount,
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
                                    addReceiptCommand.totalAmount,
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
                             "Receipt Voucher",
                            DateTime.Now,
                            "Being Receipt Vouchers recorded",
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


                    #region Payment Added
                    PaymentsDetails payment;

                    var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                       .GetByGuIdAsync(addReceiptCommand.paymentId);

                    if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                    {
                        payment = new ChequePayment(
                            id: Guid.NewGuid().ToString(),
                            transactionType: TransactionType.Receipts,
                            transactionDate: DateTime.Now,
                            totalAmount: totalAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: addReceiptCommand.paymentId,
                            chequeNumber: addReceiptCommand.chequeNumber,
                            bankName: addReceiptCommand.bankName,
                            accountName: addReceiptCommand.accountName,
                            companyId: companyId,
                            chequeDate: DateTime.Now
                        );
                    }
                    else
                    {
                        payment = new PaymentsDetails(
                            id: Guid.NewGuid().ToString(),
                            transactionType: TransactionType.Receipts,
                            transactionDate: DateTime.Now,
                            totalAmount: totalAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: addReceiptCommand.paymentId,
                            companyId: companyId
                        );
                    }

                    await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                    #endregion


                    #endregion

                    var transactionData = new TransactionDetail (
                             newId,
                             nepaliDate,
                             addReceiptCommand.totalAmount,
                             addReceiptCommand.narration,
                             companyId,
                             userId,
                             DateTime.UtcNow,
                             "",
                             default,
                             addReceiptCommand.transactionMode,
                             addReceiptCommand.paymentId,
                                newJournalId,
                                receiptNumber,
                             addReceiptCommand.transactionItemsForReceipts?.Select(d =>
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

                    if (addReceiptCommand.transactionMode != TransactionType.Receipts)
                    {
                        return Result<AddReceiptResponse>.Failure("This is not an Receipt Transactions");
                    }

                  


                    await _unitOfWork.SaveChangesAsync();

                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddReceiptResponse>(transactionData);
                    return Result<AddReceiptResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding transaction", ex);
                }
            }
        }

        public async Task<Result<ReceiptExcelResponse>> AddReceiptExcel(IFormFile formFile, CancellationToken cancellationToken = default)
        {
             using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    try
    {
        string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;
        string userId = _tokenService.GetUserId();
        var fyId = _fiscalContext.CurrentFiscalYearId;

        using var stream = new MemoryStream();
        await formFile.CopyToAsync(stream, cancellationToken).ConfigureAwait(false);

        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

        if (worksheet == null)
            return Result<ReceiptExcelResponse>.Failure("Worksheet not found");

        int rowCount = worksheet.Dimension.Rows;
        int colCount = worksheet.Dimension.Columns;

        var headerMap = new Dictionary<string, int>();
        for (int col = 1; col <= colCount; col++)
        {
            var header = worksheet.Cells[1, col].Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(header) && !headerMap.ContainsKey(header))
                headerMap[header] = col;
        }

        // Check required headers
        string[] requiredHeaders = { "entrydate", "totalamount", "narration", "transactionmode", "payment" };
        foreach (var header in requiredHeaders)
        {
            if (!headerMap.ContainsKey(header))
                return Result<ReceiptExcelResponse>.Failure($"Required header '{header}' not found");
        }

                //var allPaymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>().GetAllAsync();

                //var companyPayments = allPaymentMethods
                //    .Where(p => p.CompanyId == companyId)
                //    .ToList();
                var allPaymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>().GetAllAsync();
                var companyPayments = allPaymentMethods.ToList();

                for (int row = 2; row <= rowCount; row++)
        {
            var dateNepali = worksheet.Cells[row, headerMap["entrydate"]].Text.Trim();
            var totalAmountText = worksheet.Cells[row, headerMap["totalamount"]].Text.Trim();
            var narration = worksheet.Cells[row, headerMap["narration"]].Text.Trim();
            var transactionModeText = worksheet.Cells[row, headerMap["transactionmode"]].Text.Trim();
            var paymentNameRaw = worksheet.Cells[row, headerMap["payment"]].Text.Trim();

            if (!decimal.TryParse(totalAmountText, out var totalAmount))
                throw new Exception($"Invalid total amount at row {row}");

            var entryDate = await _dateConverterHelper.ConvertToEnglish(dateNepali);

            var transactionMode = Enum.TryParse<TransactionType>(transactionModeText, true, out var parsedMode)
                ? parsedMode
                : throw new Exception($"Invalid transaction mode at row {row}");

            var normalizedPayment = NormalizeName(paymentNameRaw);
            var matchedPayment = companyPayments.FirstOrDefault(p =>
                NormalizeName(p.Name) == normalizedPayment);

            if (matchedPayment == null)
                throw new Exception($"Payment method '{paymentNameRaw}' not found at row {row}");

            string paymentId = matchedPayment.Id;
            string transactionId = Guid.NewGuid().ToString();

            var transactionDetail = new TransactionDetail(
                transactionId,
                dateNepali,
                totalAmount,
                narration,
                companyId,
                userId,
                DateTime.UtcNow,
                "",
                default,
                transactionMode,
                paymentId,
                "",
                "",
                new List<TransactionItems>()
            );

            await _unitOfWork.BaseRepository<TransactionDetail>().AddAsync(transactionDetail).ConfigureAwait(false);

            #region Journal Entry

            string journalId = Guid.NewGuid().ToString();
            var journalDetails = new List<JournalEntryDetails>();

            var subLedgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                .GetByGuIdAsync(matchedPayment.SubLedgerGroupsId)
                ?? throw new Exception("Ledger group not found");

            var paymentLedger = await _unitOfWork.BaseRepository<Ledger>()
                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subLedgerGroup.Id)
                ?? throw new Exception("Payment ledger not found");

            journalDetails.Add(new JournalEntryDetails(
                Guid.NewGuid().ToString(),
                journalId,
                paymentLedger.Id,
                totalAmount,
                0,
                entryDate,
                companyId,
                fyId,
                true
            ));

            var journalEntry = new JournalEntry(
                journalId,
                "Receipt Voucher",
                entryDate,
                "Receipt Excel Imported",
                userId,
                companyId,
                DateTime.UtcNow,
                "",
                default,
                "",
                fyId,
                journalDetails,
                true
            );

            await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalEntry).ConfigureAwait(false);

            #endregion
        }

        await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        scope.Complete();

        return Result<ReceiptExcelResponse>.Success($"{rowCount - 1} receipts imported successfully.");
    }
    catch (Exception ex)
    {
        // preserve stack trace
        throw new Exception("An error occurred while importing receipts.", ex);
    }
        }

        private string NormalizeName(string paymentNameRaw)
        {
            if (string.IsNullOrWhiteSpace(paymentNameRaw))
                return string.Empty;

            return paymentNameRaw.Trim().ToLower().Replace(" ", "");
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _tokenService.GetUserId();

             
                var receiptDetailsList = await _unitOfWork.BaseRepository<TransactionDetail>()
                   .GetConditionalAsync(x => x.Id == id && x.TransactionMode == TransactionDetail.TransactionType.Receipts);

                var singleReceiptDetail = receiptDetailsList.FirstOrDefault();
                if (singleReceiptDetail is null)
                {
                    return Result<bool>.Failure("NotFound", "Receipt cannot be found");
                }

             
                if (!string.IsNullOrEmpty(singleReceiptDetail.JournalEntriesId))
                {
                    var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(x => x.Id == singleReceiptDetail.JournalEntriesId,
                            query => query.Include(j => j.JournalEntryDetails)
                        );

                    var originalJournal = originalJournals.FirstOrDefault();
                    if (originalJournal is not null)
                    {
                        _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                    }
                }

             
                _unitOfWork.BaseRepository<TransactionDetail>().Delete(singleReceiptDetail);

             
                await _unitOfWork.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting receipt having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllReceiptQueryResponse>>> GetAll(PaginationRequest paginationRequest,string? ledgerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var receipt = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(
                         x => x.TransactionMode == TransactionType.Receipts && (string.IsNullOrEmpty(ledgerId)
                         || x.TransactionsItems.Any(ti => ti.LedgerId == ledgerId)),
                query => query.Include(t => t.TransactionsItems).Include(t => t.PaymentMethods)
                    );
                var transactionIds = receipt.Select(t => t.Id).ToList();
                var chequeReceipts = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => transactionIds.Contains(x.TransactionDetailsId) &&
                                              x.TransactionType == TransactionType.Receipts);

                var receiptResponse = receipt.Select(t => 
                {
                    var cheque = chequeReceipts.FirstOrDefault(c => c.TransactionDetailsId == t.Id);
                    return new GetAllReceiptQueryResponse(
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
                }
                ).ToList();

                var totalItems = receiptResponse.Count;

                var paginatedReceipts = paginationRequest != null && paginationRequest.IsPagination
                    ? receiptResponse
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : receiptResponse;

                var pagedResult = new PagedResult<GetAllReceiptQueryResponse>
                {
                    Items = paginatedReceipts,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllReceiptQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetAllReceiptQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<GetFilterReceiptQueryRespopnse>>> GetFilterReceipt(PaginationRequest paginationRequest,FilterReceiptDto filterReceiptDto)
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

                if (filterReceiptDto.startDate != default)
                    startEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterReceiptDto.startDate);

                if (filterReceiptDto.endDate != default)
                    endEnglishDate = await _dateConverterHelper.ConvertToEnglish(filterReceiptDto.endDate);


                if (string.IsNullOrEmpty(filterReceiptDto.ledgerId) && startEnglishDate == null && endEnglishDate == null)
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
                var receiptResult = await _unitOfWork.BaseRepository<TransactionDetail>().GetConditionalAsync(
              x =>
                  x.CreatedBy == userId &&
                  x.TransactionMode == TransactionType.Receipts &&
                  (string.IsNullOrEmpty(filterReceiptDto.ledgerId) ||
                      x.TransactionsItems.Any(ti => ti.LedgerId == filterReceiptDto.ledgerId)) && 
                  (startEnglishDate == null || x.CreatedAt >= startEnglishDate) &&
                  (endEnglishDate == null || x.CreatedAt <= endEnglishDate),
              q => q.Include(sd => sd.TransactionsItems)
          );


                var receiptIds = receiptResult.Select(r => r.Id).ToList();
                var paymentDetailsList = await _unitOfWork.BaseRepository<PaymentsDetails>()
                    .GetConditionalAsync(x => receiptIds.Contains(x.TransactionDetailsId)
                                      && x.TransactionType == TransactionType.Receipts);


                var paymentDetailsDict = paymentDetailsList
                    .GroupBy(p => p.TransactionDetailsId)
                    .ToDictionary(g => g.Key, g => g.FirstOrDefault());


                var responseList = receiptResult.Select(r =>
                {
                    var paymentDetails = paymentDetailsDict.ContainsKey(r.Id)
                        ? paymentDetailsDict[r.Id]
                        : null;

                    var chequePayment = paymentDetails as ChequePayment;

                    var chequeNumber = chequePayment?.ChequeNumber ?? null;
                    var bankName = chequePayment?.BankName ?? null;
                    var accountName = chequePayment?.AccountName ?? null;
                    var chequeDate = chequePayment?.ChequeDate;

                    return new GetFilterReceiptQueryRespopnse(
                        r.Id,
                        r.TransactionDate,
                        r.TotalAmount,
                        r.Narration,
                        r.TransactionMode,
                        r.PaymentMethodId,
                        chequeNumber,
                        r.TransactionsItems?.Select(m => new AddTransactionItemsRequest(
                            m.Id,
                            m.Amount,
                            m.Remarks,
                            m.LedgerId
                        )).ToList() ?? new List<AddTransactionItemsRequest>()
                    );
                }).ToList();


                PagedResult<GetFilterReceiptQueryRespopnse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetFilterReceiptQueryRespopnse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetFilterReceiptQueryRespopnse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }


                return Result<PagedResult<GetFilterReceiptQueryRespopnse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching receipts: {ex.Message}", ex);
            }

        }

        public async Task<Result<GetReceiptByIdQueryResponse>> GetReceiptById(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                var receiptData = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id,
                        query => query.Include(td => td.TransactionsItems)
                    );

                var td = receiptData.FirstOrDefault(); // ✅ single receipt expected

                if (td == null)
                    return Result<GetReceiptByIdQueryResponse>.Failure("Receipt not found.");

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>().FirstOrDefaultAsync(x => x.TransactionDetailsId == td.Id);


                var paymentsDetailsData = await _unitOfWork.BaseRepository<ChequePayment>()
                .GetConditionalAsync(x => x.TransactionDetailsId == id
                    && x.TransactionType == TransactionType.Payment);

                var paymentsStatusData = paymentsDetailsData.FirstOrDefault();




                var receiptResponse = new GetReceiptByIdQueryResponse(
                    td.Id,
                    td.TransactionDate,
                    td.TotalAmount,
                    td.Narration,
                    td.TransactionMode,
                    td.TransactionNumber,
                    paymentDetails?.PaymentMethodId,
                    paymentsStatusData?.ChequeNumber,
                    paymentsStatusData?.BankName,
                    paymentsStatusData?.AccountName,
                    td.TransactionsItems?.Select(detail => new UpdateTransactionItemRequest(
                
                
                        detail.Amount,
                        detail.Remarks,
                        detail.LedgerId
                    )).ToList() ?? new List<UpdateTransactionItemRequest>()
                );

                return Result<GetReceiptByIdQueryResponse>.Success(receiptResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching receipt by Id", ex);
            }
        }

        public async Task<Result<UpdateReceiptResponse>> Update(UpdateReceiptCommand updateReceiptCommand, string id)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Result<UpdateReceiptResponse>.Failure("InvalidRequest", "Receipt ID cannot be null or empty");
                }

                var receiptToUpdate = await _unitOfWork.BaseRepository<TransactionDetail>()
                    .GetConditionalAsync(x => x.Id == id, q => q.Include(t => t.TransactionsItems));

                var receipt = receiptToUpdate.FirstOrDefault();

                if (receipt is null)
                {
                    return Result<UpdateReceiptResponse>.Failure("NotFound", "Receipt not found.");
                }

                
                _mapper.Map(updateReceiptCommand, receipt);





                if (updateReceiptCommand.TransactionsItems != null)
                {

                    receipt.TransactionsItems.Clear();


                    foreach (var itemDto in updateReceiptCommand.TransactionsItems)
                    {
                        var newItem = _mapper.Map<TransactionItems>(itemDto);
                        newItem.Id = Guid.NewGuid().ToString(); // new Id
                        newItem.TransactionDetailId = id;
                        newItem.Amount = itemDto.amount;
                        newItem.Remarks = itemDto.remarks;
                        newItem.LedgerId = itemDto.ledgerId;

                        receipt.TransactionsItems.Add(newItem); // EF tracks automatically
                    }

                    await _unitOfWork.SaveChangesAsync();
                }


                await _unitOfWork.SaveChangesAsync();


                var oldJournal = await _unitOfWork.BaseRepository<JournalEntry>()
                      .FirstOrDefaultAsync(j => j.Id == receipt.JournalEntriesId);

                if (oldJournal != null)
                {
                    //oldJournal.IsDeleted = true;
                    _unitOfWork.BaseRepository<JournalEntry>().Delete(oldJournal);
                    await _unitOfWork.SaveChangesAsync();
                }

                #region Update PaymentDetails

                var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                    .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Receipts);

                paymentDetails.TotalAmount = updateReceiptCommand.totalAmount;
                if (paymentDetails is not null)
                {
                    _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                }

                #endregion
                DateTime englishDate = await _dateConverterHelper.ConvertToEnglish(updateReceiptCommand.transactionDate);

                string newJournalId = Guid.NewGuid().ToString();
                var journalDetails = new List<JournalEntryDetails>();

                foreach (var item in updateReceiptCommand.TransactionsItems)
                {
                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        item.ledgerId,
                        0,                 // Debit
                        item.amount,       // Credit
                        englishDate,
                        receipt.CompanyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));
                }

                if (updateReceiptCommand.paymentId != null)
                {
                    var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                        .GetByGuIdAsync(updateReceiptCommand.paymentId);

                    var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                        .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId) ?? throw new Exception("ledger group not found.");

                    var ledgers = await _unitOfWork.BaseRepository<Ledger>()
                        .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        ledgers.Id,
                        updateReceiptCommand.totalAmount, // Debit
                        0,                                // Credit
                        englishDate,
                        receipt.CompanyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));
                }

                var journalData = new JournalEntry(
                        newJournalId,
                        "Receipt Voucher",
                        englishDate,
                        "Being Receipt Voucher updated",
                        _tokenService.GetUserId(),
                        receipt.CompanyId,
                        DateTime.UtcNow,
                        "",
                        default,
                        "",
                        _fiscalContext.CurrentFiscalYearId,
                        journalDetails,
                        true
                    );


                if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                {
                    throw new InvalidOperationException("Journal entry is unbalanced.");
                }

                await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                #region Payment Added
                PaymentsDetails payment;

                var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                   .GetByGuIdAsync(updateReceiptCommand.paymentId);

                if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                {
                    payment = new ChequePayment(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Income,
                        transactionDate: DateTime.Now,
                        totalAmount: updateReceiptCommand.totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updateReceiptCommand.paymentId,
                        chequeNumber: updateReceiptCommand.chequeNumber,
                        bankName: updateReceiptCommand.bankName,
                        accountName: updateReceiptCommand.accountName,
                        companyId: receipt.CompanyId,
                        chequeDate: DateTime.Now
                    );
                }
                else
                {
                    payment = new PaymentsDetails(
                        id: Guid.NewGuid().ToString(),
                        transactionType: TransactionType.Income,
                        transactionDate: DateTime.Now,
                        totalAmount: updateReceiptCommand.totalAmount,
                        transactionDetailsId: newJournalId,
                        paymentMethodId: updateReceiptCommand.paymentId,
                        companyId: receipt.CompanyId
                    );
                }

                await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                #endregion



                scope.Complete();

                // Build response
                var resultResponse = new UpdateReceiptResponse(
                   
                   updateReceiptCommand.transactionDate,
                   updateReceiptCommand.totalAmount,
                   updateReceiptCommand.narration,
                   updateReceiptCommand.transactionMode,
                   updateReceiptCommand.TransactionsItems.Select(d => new UpdateTransactionItemRequest(
                 
                 
                          d.amount,
                          d.remarks,
                          d.ledgerId
                         
         
                        )).ToList()
                );

                return Result<UpdateReceiptResponse>.Success(resultResponse);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the receipt.", ex);
            }
        }
    }
    
}

    

