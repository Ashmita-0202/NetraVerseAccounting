using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NV.Payment.Domain.Entities;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Transactions;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Command.AddSalesItems;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany;
using TN.Sales.Application.Sales.Command.UpdateSalesDetails;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.BillNumberGenerationByCompany;
using TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.Sales.Queries.GetAllSalesItems;
using TN.Sales.Application.Sales.Queries.GetSalesDetailsByRefNo;
using TN.Sales.Application.Sales.Queries.GetSalesQuotationById;
using TN.Sales.Application.Sales.Queries.SalesDetailsById;
using TN.Sales.Application.Sales.Queries.SalesItemByItemId;
using TN.Sales.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.CalculationBillSundry;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.Entities.Purchase;
using TN.Shared.Domain.Entities.Sales;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Authentication.Domain.Entities.Company;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Sales.Domain.Entities.SalesDetails;
using static TN.Shared.Domain.Entities.Sales.SalesQuotationDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Sales.Infrastructure.ServiceImpl
{
    public class SalesDetailsServices : ISalesDetailsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IMediator _mediator;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly IDateConvertHelper _dateConvertHelper;
        private readonly IInventoryMethodType _inventoryMethodType;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        private readonly IBillSundryServices _billSundryServices;


        public SalesDetailsServices(IBillSundryServices billSundryServices,IInventoryMethodType inventoryMethodType,ISettingServices settingServices,FiscalContext fiscalContext,IGetUserScopedData getUserScopedData,IMediator mediator,IBillNumberGenerator billNumberGenerator ,IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _mediator = mediator;
            _getUserScopedData = getUserScopedData;
            _billNumberGenerator = billNumberGenerator;
            _dateConvertHelper= dateConvertHelper;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _inventoryMethodType = inventoryMethodType;
            _fiscalContext = fiscalContext;
            _settingServices = settingServices;
            _billSundryServices = billSundryServices;

        }
        public async Task<Result<AddSalesDetailsResponse>> Add(AddSalesDetailsCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;

                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);
                    var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);

                    if (company == null)
                    {
                        return Result<AddSalesDetailsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }


                    string billNumber = "";
                    if (company.BillNumberGenerationTypeForSales == BillNumberGenerationType.Manual)
                    {
                        billNumber = request.billNumber;
                    }
                    else
                    {
                        billNumber = await _billNumberGenerator.GenerateBillNumberAsync(companyId, "sales",fiscalYear.Data.fyName);
                    }
                    bool billNumberExists = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .AnyAsync(p => p.BillNumber == billNumber && p.CompanyId == companyId);

                    bool billNumberExistsInSales = await _unitOfWork.BaseRepository<SalesDetails>()
                        .AnyAsync(s => s.BillNumber == billNumber && s.CompanyId == companyId);

                    if (billNumberExists || billNumberExistsInSales)
                    {
                        return Result<AddSalesDetailsResponse>.Failure($"Bill number '{billNumber}' already exists for this company.");
                    }

                    DateTime entryDate = request.date == null
                      ? DateTime.Today
                      : await _dateConvertHelper.ConvertToEnglish(request.date);



                    string newJournalId = "";
                    var resultDTOs = new AddSalesDetailsResponse();
                    if (request.isSales)
                    {
                        newJournalId = Guid.NewGuid().ToString();



                        // Base amounts
                        decimal subTotal = request.subTotalAmount ?? 0;
                        decimal vatAmount = request.vatAmount ?? 0;
                        decimal discount = request.discountAmount ?? 0;
                        decimal taxableAmount = request.taxableAmount ?? 0;
                        decimal amountAfterVat = request.amountAfterVat ?? 0;

    
                        decimal billSundrySalesTotal = 0;     
                        decimal billSundryCustomerTotal = 0;  

                        var journalDetails = new List<JournalEntryDetails>();

                        var billSundryResponses = new List<CalculationBillResponseDTOs>();
                        var billSundryDetailsList = new List<BillSundryDetails>();


                        if (request.BillSundryIds != null && request.BillSundryIds.Any())
                        {
                            foreach (var bsId in request.BillSundryIds.Where(x => !string.IsNullOrEmpty(x.billSundryIds)))
                            {
                                var calcDto = new CalculationBIllDTOs(bsId.billSundryIds, subTotal, taxableAmount, amountAfterVat);
                                var bsConfig = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(bsId.billSundryIds);
                                bsConfig.DefaultValue = bsId.rate ?? 0;
                                _unitOfWork.BaseRepository<BillSundry>().Update(bsConfig);


                                //Add BillSundryDetails
                                var billSundryDetails = new BillSundryDetails
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BillSundryId = bsId.billSundryIds,
                                    Rate = bsConfig.DefaultValue ?? 0,
                                    PurchaseSalesAndReturnId = newId,
                                };

                                billSundryDetailsList.Add(billSundryDetails);




                                var calcResult = await _billSundryServices.CalculateBillSundry(calcDto);
                                if (!calcResult.IsSuccess)
                                    return Result<AddSalesDetailsResponse>.Failure("Calculation Error");

                                var bsResponse = calcResult.Data;

                                if (bsConfig.IsSalesAccountingAffected)
                                {
                                    // Credit Sales Ledger
                                    decimal creditAmount = !bsConfig.IsSalesAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                    decimal debitAmount = !bsConfig.CustomerAmountAdjusted ? bsResponse.CalculatedAmount : 0;

                                    if (creditAmount > 0)
                                    {
                                        journalDetails.Add(new JournalEntryDetails(
                                            Guid.NewGuid().ToString(),
                                            newJournalId,
                                            bsConfig.SalesAdjustedLedgerId,
                                            0,
                                            creditAmount,
                                            entryDate,
                                            companyId,
                                            FyId,
                                            true
                                        ));
                                        billSundrySalesTotal += creditAmount;
                                    }

                                    // Debit Customer Ledger
                                    if (debitAmount > 0)
                                    {
                                        journalDetails.Add(new JournalEntryDetails(
                                            Guid.NewGuid().ToString(),
                                            newJournalId,
                                            bsConfig.CustomerAdjustedLedgerId,
                                            debitAmount,
                                            0,
                                            entryDate,
                                            companyId,
                                            FyId,
                                            true
                                        ));
                                        billSundryCustomerTotal += debitAmount;
                                    }
                                }

                                await _unitOfWork.BaseRepository<BillSundryDetails>().AddRange(billSundryDetailsList);
                            }
                        }



                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.SalesLedgerId,
                            0,
                            subTotal,
                            entryDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));


                        if (vatAmount > 0)
                        {
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.VATLedgerId,
                                0,
                                vatAmount,
                                entryDate,
                                companyId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

                        if (discount > 0)
                        {
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                LedgerConstants.DiscountLedgerId,
                                discount,
                                0,
                                entryDate,
                                companyId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

        
                        string ledgerIdToUse = null;
                        var hasLedger = !string.IsNullOrEmpty(request.ledgerId);
                        var hasPayment = !string.IsNullOrEmpty(request.paymentId);
                        var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                        var isSpecial = request.paymentId == specialPaymentId;

                        if (isSpecial && hasLedger)
                            ledgerIdToUse = request.ledgerId;
                        else if (hasPayment)
                        {
                            var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(request.paymentId);
                            var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                                .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                                ?? throw new Exception("Ledger Group not found");

                            var ledger = await _unitOfWork.BaseRepository<Ledger>()
                                .FirstOrDefaultAsync(x => x.SubLedgerGroupId == subledgerGroup.Id);

                            ledgerIdToUse = subledgerGroup.Id switch
                            {
                                SubLedgerGroupConstants.CashInHands => ledger?.Id ?? LedgerConstants.CashLedgerId,
                                SubLedgerGroupConstants.BankAccounts => ledger?.Id ?? LedgerConstants.BankLedgerId,
                                _ => ledger?.Id ?? request.ledgerId
                            };
                        }
                        else if (hasLedger)
                            ledgerIdToUse = request.ledgerId;

                        decimal totalCreditSoFar = journalDetails.Sum(x => x.CreditAmount);
                        decimal totalDebitSoFar = journalDetails.Sum(x => x.DebitAmount);
                        decimal customerDebit = totalCreditSoFar - totalDebitSoFar;

                        if (!string.IsNullOrEmpty(ledgerIdToUse))
                        {
                            journalDetails.Add(new JournalEntryDetails(
                                Guid.NewGuid().ToString(),
                                newJournalId,
                                ledgerIdToUse,
                                customerDebit,
                                0,
                                entryDate,
                                companyId,
                                _fiscalContext.CurrentFiscalYearId,
                                true
                            ));
                        }

      
                        decimal totalDebit = journalDetails.Sum(x => x.DebitAmount);
                        decimal totalCredit = journalDetails.Sum(x => x.CreditAmount);

                        if (totalDebit != totalCredit)
                            throw new InvalidOperationException($"Journal unbalanced! Debit={totalDebit}, Credit={totalCredit}");

                        var journalData = new JournalEntry(
                            newJournalId,
                            "Sales Voucher",
                            entryDate,
                            "Being Item Sold",
                            userId,
                            companyId,
                            DateTime.UtcNow,
                            "",
                            default,
                            request.billNumber,
                            FyId,
                            journalDetails,
                            true
                        );

                        await _unitOfWork.BaseRepository<JournalEntry>().AddAsync(journalData);



                        string nepaliDate = await _dateConvertHelper.ConvertToNepali(entryDate);

                        var salesDetailsData = new SalesDetails
                        (
                                newId,
                                nepaliDate,
                                billNumber,
                                request.ledgerId,
                                request.amountInWords,
                                request.discountPercent,
                                request.discountAmount,
                                request.vatPercent,
                                request.vatAmount,
                                userId,
                                companyId,
                                DateTime.UtcNow,
                                "",
                                default,
                                request.grandTotalAmount,
                                SalesStatus.Settled,
                                newJournalId,

                                request.paymentId,
                                true,
                                request.StockCenterId,
                                true,
                                request.subTotalAmount,
                                request.taxableAmount,
                                request.amountAfterVat,
                            request.SalesItems?.Select(d => new SalesItems
                              (
                                  Guid.NewGuid().ToString(),
                                  d.quantity,
                                  d.unitId,
                                  d.itemId,
                                  d.price,
                                  d.amount,
                                  userId,
                                  DateTime.Now.ToString(),
                                  "",
                                  "",
                                  newId,
                                  true
                              )).ToList() ?? new List<SalesItems>().ToList()
                        );


                        decimal cogs = await _inventoryMethodType.ProcessInventoryMethod(companyId, request.ledgerId, request.SalesItems, salesDetailsData);

                        _inventoryMethodType.JournalCOGSEntry(cogs, entryDate, userId, companyId);



                        await _unitOfWork.BaseRepository<SalesDetails>().AddAsync(salesDetailsData);
             


                        #region Subtract as per itemSell


                        #endregion

                        // for serialNumber
                        var itemInstances = new List<ItemInstance>();

                        foreach (var salesItem in request.SalesItems)
                        {

                            var matchedSavedItem = salesDetailsData.SalesItems
                                .FirstOrDefault(x => x.ItemId == salesItem.itemId);

                            if (matchedSavedItem == null)
                                continue;

                            int quantityToAssign = Convert.ToInt32(salesItem.quantity);
                            var serials = salesItem.serialNumbers ?? new List<string>();

                            int serialCount = Convert.ToInt32(serials.Count);
                            int instanceCount = (int)Math.Min(quantityToAssign, serialCount);





                            //int instanceCount = (int)Math.Min(salesItem.quantity, salesItem.serialNumbers.Count);

                            for (int i = 0; i < instanceCount; i++)
                            {
                                string serial = salesItem.serialNumbers[i];

                                var existingInstance = await _unitOfWork
                                 .BaseRepository<ItemInstance>()
                                 .FirstOrDefaultAsync(x =>
                                     x.ItemsId == salesItem.itemId &&
                                     x.SalesItemsId == null &&
                                     x.SerialNumber == serial);


                                if (existingInstance != null)
                                {
                                    existingInstance.SerialNumber = serial;
                                    existingInstance.SalesItemsId = matchedSavedItem.Id;
                                    existingInstance.Date = DateTime.UtcNow;
                                    _unitOfWork.BaseRepository<ItemInstance>().Update(existingInstance);
                                }
                                else
                                {
                                    var newItemInstance = new ItemInstance
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ItemsId = salesItem.itemId,
                                        SerialNumber = serial,
                                        PurchaseItemsId = null,
                                        SalesItemsId = matchedSavedItem.Id,
                                        Rate = salesItem.price,
                                        Date = DateTime.UtcNow
                                    };

                                    itemInstances.Add(newItemInstance);
                                }
                            }

                            // 2. Handle remaining quantity (no serials)
                            int remainingQuantity = quantityToAssign - instanceCount;
                            if (remainingQuantity > 0)
                            {
                                var instancesToUpdate = await _unitOfWork
                                    .BaseRepository<ItemInstance>()
                                    .GetConditionalAsync(x =>
                                        x.ItemsId == salesItem.itemId &&
                                        x.SalesItemsId == null);

                                foreach (var instance in instancesToUpdate.Take(remainingQuantity))
                                {
                                    instance.SalesItemsId = matchedSavedItem.Id;
                                    instance.Date = DateTime.UtcNow;
                                    _unitOfWork.BaseRepository<ItemInstance>().Update(instance);
                                }
                            }

                        }

                        if (itemInstances.Any())
                        {
                            await _unitOfWork.BaseRepository<ItemInstance>().AddRange(itemInstances);
                        }




                        #region Payment Added
                        decimal totalAmount = subTotal + vatAmount - discount + billSundryCustomerTotal;

                        PaymentsDetails payment;

                        var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                            .GetByGuIdAsync(request.paymentId);

                        if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                        {
                            payment = new ChequePayment(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Sales,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,  // full amount, not just billSundryTotal
                                transactionDetailsId: newId,
                                paymentMethodId: request.paymentId,
                                chequeNumber: request.chequeNumber,
                                bankName: request.bankName,
                                accountName: request.accountName,
                                companyId: companyId,
                                chequeDate: DateTime.Now
                            );
                        }
                        else
                        {
                            payment = new PaymentsDetails(
                                id: Guid.NewGuid().ToString(),
                                transactionType: TransactionType.Sales,
                                transactionDate: DateTime.Now,
                                totalAmount: totalAmount,  // full amount
                                transactionDetailsId: newId,
                                paymentMethodId: request.paymentId,
                                companyId: companyId
                            );
                        }

                        await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);


                        #endregion

                        var mappedSalesItems = request.SalesItems.Select(x => new AddSalesItemsRequest
                            (
                                x.quantity,
                                x.unitId,
                                x.itemId,
                                x.price,
                                x.amount,
                                x.serialNumbers?
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .Select(s => s.Length > 50 ? s.Substring(0, 50) : s)
                                    .ToList() ?? new List<string>()
                            )).ToList();



                        var response = new AddSalesDetailsResponse(
                                salesDetailsData.Id,
                                salesDetailsData.Date,
                                salesDetailsData.BillNumber,
                                salesDetailsData.LedgerId,
                                salesDetailsData.AmountInWords,
                                salesDetailsData.DiscountPercent ?? 0,
                                salesDetailsData.DiscountAmount ?? 0,
                                salesDetailsData.VatPercent ?? 0,
                                salesDetailsData.VatAmount ?? 0,
                                salesDetailsData.CreatedBy,
                                salesDetailsData.CompanyId,
                                salesDetailsData.CreatedAt,
                                salesDetailsData.GrandTotalAmount,
                                salesDetailsData.PaymentId,
                                salesDetailsData.StockCenterId,
                                request.subTotalAmount,
                                request.taxableAmount,
                                request.amountAfterVat,
                                mappedSalesItems
                            );



                        #region Delete or Soft Delete Quotation if converted to sales

                        if(!string.IsNullOrEmpty(request.salesQuotationNumber))
                        {
                            var quotationData = await _unitOfWork.BaseRepository<SalesQuotationDetails>()
                       .FirstOrDefaultAsync(x => x.SalesQuotationNumber == request.salesQuotationNumber);
                            quotationData.QuotationStatuss = QuotationStatus.Converted;

                            _unitOfWork.BaseRepository<SalesQuotationDetails>().Update(quotationData);
                        }

                  

                        #endregion




                        resultDTOs = _mapper.Map<AddSalesDetailsResponse>(response);

                    }
                    else
                    {
                        var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                        if (companySettings == null)
                        {
                            return Result<AddSalesDetailsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                        }

                        string salesQuotationNumber = "";
                        if (companySettings.SalesQuotationNumberType == PurchaseSalesQuotationNumberType.Manual)
                        {
                            salesQuotationNumber = request.salesQuotationNumber!;
                        }
                        else
                        {
                            salesQuotationNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "SALESQUOTATION", fiscalYear.Data.fyName);
                        }



                        bool purchaseQuotationNumberExists = await _unitOfWork.BaseRepository<SalesQuotationDetails>()
                        .AnyAsync(p => p.SalesQuotationNumber == salesQuotationNumber && p.CompanyId == companyId);



                        if (purchaseQuotationNumberExists)
                        {
                            return Result<AddSalesDetailsResponse>.Failure($"SalesQuotation number '{salesQuotationNumber}' already exists for this company.");
                        }


                        string nepaliDate = await _dateConvertHelper.ConvertToNepali(entryDate);
                        var salesQuotationDetailsData = new SalesQuotationDetails
                        (
                                newId,
                                nepaliDate,
                                request.billNumber,
                                request.ledgerId,
                                request.amountInWords,
                                request.discountPercent,
                                request.discountAmount,
                                request.vatPercent,
                                request.vatAmount,
                                userId,
                                companyId,
                                DateTime.UtcNow,
                                "",
                                default,
                                request.grandTotalAmount,
                                true,   
                                request.StockCenterId,
                                salesQuotationNumber,
                                request.subTotalAmount,
                                request.taxableAmount,
                                request.amountAfterVat,
                                QuotationStatus.Pending,
                            request.SalesItems?.Select(d => new SalesQuotationItems
                              (
                                  Guid.NewGuid().ToString(),
                                  d.quantity,
                                  d.unitId,
                                  d.itemId,
                                  d.price,
                                  d.amount,
                                  userId,
                                  DateTime.Now.ToString(),
                                  "",
                                  "",
                                  newId,
                                  true
                              )).ToList() ?? new List<SalesQuotationItems>().ToList()
                        );

                        await _unitOfWork.BaseRepository<SalesQuotationDetails>().AddAsync(salesQuotationDetailsData);



                        // for serialNumber
                        var itemInstances = new List<ItemInstance>();

                        foreach (var salesQuotationItem in request.SalesItems)
                        {

                            var matchedSavedItem = salesQuotationDetailsData.SalesQuotationItems
                                .FirstOrDefault(x => x.ItemId == salesQuotationItem.itemId);

                            if (matchedSavedItem == null)
                                continue;

                            int quantityToAssign = Convert.ToInt32(salesQuotationItem.quantity);
                            var serials = salesQuotationItem.serialNumbers ?? new List<string>();

                            int serialCount = Convert.ToInt32(serials.Count);
                            int instanceCount = (int)Math.Min(quantityToAssign, serialCount);





                            //int instanceCount = (int)Math.Min(salesItem.quantity, salesItem.serialNumbers.Count);

                            for (int i = 0; i < instanceCount; i++)
                            {
                                string serial = salesQuotationItem.serialNumbers[i];

                                var existingInstance = await _unitOfWork
                                 .BaseRepository<ItemInstance>()
                                 .FirstOrDefaultAsync(x =>
                                     x.ItemsId == salesQuotationItem.itemId &&
                                     x.SalesQuotationItemsId == null &&
                                     x.SerialNumber == serial);


                                if (existingInstance != null)
                                {
                                    existingInstance.SerialNumber = serial;
                                    existingInstance.SalesQuotationItemsId = matchedSavedItem.Id;
                                    existingInstance.Date = DateTime.UtcNow;
                                    _unitOfWork.BaseRepository<ItemInstance>().Update(existingInstance);
                                }
                                else
                                {
                                    var newItemInstance = new ItemInstance
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        ItemsId = salesQuotationItem.itemId,
                                        SerialNumber = serial,
                                        PurchaseItemsId = null,
                                        SalesQuotationItemsId = matchedSavedItem.Id,
                                        Date = DateTime.UtcNow
                                    };

                                    itemInstances.Add(newItemInstance);
                                }
                            }

                            // 2. Handle remaining quantity (no serials)
                            int remainingQuantity = quantityToAssign - instanceCount;
                            if (remainingQuantity > 0)
                            {
                                var instancesToUpdate = await _unitOfWork
                                    .BaseRepository<ItemInstance>()
                                    .GetConditionalAsync(x =>
                                        x.ItemsId == salesQuotationItem.itemId &&
                                        x.SalesQuotationItemsId == null);

                                foreach (var instance in instancesToUpdate.Take(remainingQuantity))
                                {
                                    instance.SalesQuotationItemsId = matchedSavedItem.Id;
                                    instance.Date = DateTime.UtcNow;
                                    _unitOfWork.BaseRepository<ItemInstance>().Update(instance);
                                }
                            }

                        }

                        if (itemInstances.Any())
                        {
                            await _unitOfWork.BaseRepository<ItemInstance>().AddRange(itemInstances);
                        }





                        resultDTOs = new AddSalesDetailsResponse(
                                Id: salesQuotationDetailsData.Id,
                                Date: salesQuotationDetailsData.Date,
                                BillNumber: salesQuotationDetailsData.QuotationNumber,
                                LedgerId: salesQuotationDetailsData.LedgerId,
                                AmountInWords: salesQuotationDetailsData.AmountInWords,
                                SubTotalAmount: salesQuotationDetailsData.DiscountPercent ?? 0,
                                TaxableAmount: salesQuotationDetailsData.DiscountAmount ?? 0,
                                AmountAfterVat: salesQuotationDetailsData.VatPercent ?? 0,
                                CreatedBy: salesQuotationDetailsData.CreatedBy,
                                CompanyId: salesQuotationDetailsData.CompanyId,
                                CreatedAt: salesQuotationDetailsData.CreatedAt,
                                GrandTotalAmount: salesQuotationDetailsData.GrandTotalAmount,
                                StockCenterId: salesQuotationDetailsData.StockCenterId ?? "",
                                SalesItems: salesQuotationDetailsData.SalesQuotationItems.Select(item => new AddSalesItemsRequest(
                                    quantity: item.Quantity,
                                    unitId: item.UnitId,
                                    itemId: item.ItemId,
                                    price: item.Price,
                                    amount: item.Amount,
                                    serialNumbers: null // or pass mapped serial numbers if available
                                )).ToList()
                            );

                    }



                   
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();
                    

                    return Result<AddSalesDetailsResponse>.Success(resultDTOs);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    if (ex is AggregateException aggEx)
                    {
                        foreach (var inner in aggEx.InnerExceptions)
                        {
                            Console.WriteLine($"Inner Exception: {inner.Message}");
                        }
                    }
                    scope.Dispose();
                    throw new Exception("An error occurred while adding salesDetails", ex);
                }
            }
        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {

                var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>().GetByGuIdAsync(id);
                if (salesDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "salesDetails  Cannot be Found");
                }

                var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                 .GetConditionalAsync(x => x.Id == salesDetails.JournalEntriesId,
                 query => query.Include(j => j.JournalEntryDetails)
                 );
                var originalJournal = originalJournals.FirstOrDefault();
                _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);

                _unitOfWork.BaseRepository<SalesDetails>().Delete(salesDetails);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting sales Details having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesDetailsByQueryResponse>>> GetAllSalesDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
             .GetConditionalAsync(
                 x=>x.IsActive == true && x.Status != SalesStatus.Returned,
                 query => query
                     .Include(s => s.SalesItems.Where(pi => pi.IsActive))
                         .ThenInclude(si => si.ItemInstances)
                     .Include(s => s.SalesItems.Where(pi=>pi.IsActive))
                         .ThenInclude(si => si.Item)
             ) ?? new List<SalesDetails>();

                var salesDetailsResponses = salesDetails.Select(Sales => new GetAllSalesDetailsByQueryResponse(
                    Sales.Id,
                    Sales.Date,
                    Sales.BillNumber,
                    Sales.LedgerId,
                    Sales.AmountInWords,
                    Sales.DiscountPercent ?? 0,
                    Sales.DiscountAmount ?? 0,
                    Sales.VatPercent ?? 0,
                    Sales.VatAmount ?? 0,
                    Sales.CompanyId,
                    Sales.Status,
                    Sales.GrandTotalAmount,
                    Sales.PaymentId,
                    Sales.StockCenterId,
                    Sales.SalesItems?.Select(items => new SalesItemsDto(
                        items.Id,
                        items.Quantity,
                        items.UnitId,
                        items.ItemId,
                        items.Price,
                        items.Amount,
                        items.CreatedBy,
                        items.CreatedAt,
                        items.UpdatedBy,
                        items.UpdatedAt,
                        items.Item.HsCode ?? "",
                        items.Item.IsVatEnables,
                       items.ItemInstances?.Where(x=>!string.IsNullOrWhiteSpace(x.SerialNumber)).Select(x => x.SerialNumber).ToList() ?? new List<string>()
                   
                    )).ToList() ?? new List<SalesItemsDto>()
                )).ToList();

                var institutionId = _tokenService.InstitutionId() ?? string.Empty;
                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;

                var filterSales = isSuperAdmin
                    ? salesDetailsResponses 
                    : salesDetailsResponses.Where(x=>x.companyId == _tokenService.CompanyId().FirstOrDefault()).ToList();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                   .GetConditionalFilterType(
                       x => x.InstitutionId == institutionId,
                       query => query.Select(c => c.Id)
                   );

                if(!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(_tokenService.CompanyId().FirstOrDefault()))
                {
                    var filteredEntries = await _unitOfWork.BaseRepository<SalesDetails>()
                        .GetConditionalAsync(x => companyIds.Contains(x.CompanyId),
                        query => query.Include(j => j.SalesItems).ThenInclude(x=>x.Item));

                    filterSales = filteredEntries.Select(Sales => new GetAllSalesDetailsByQueryResponse(
                    Sales.Id,
                    Sales.Date,
                    Sales.BillNumber,
                    Sales.LedgerId,
                    Sales.AmountInWords,
                    Sales.DiscountPercent,
                    Sales.DiscountAmount,
                    Sales.VatPercent,
                    Sales.VatAmount,
                    Sales.CompanyId,
                    Sales.Status,
                    Sales.GrandTotalAmount,
                    Sales.PaymentId,
                    Sales.StockCenterId,
                    Sales.SalesItems?.Select(items => new SalesItemsDto(
                        items.Id,
                        items.Quantity,
                        items.UnitId,
                        items.ItemId,
                        items.Price,
                        items.Amount,
                        items.CreatedBy,
                        items.CreatedAt,
                        items.UpdatedBy,
                        items.UpdatedAt,
                        items.Item.HsCode ?? "",
                        items.Item.IsVatEnables,
                        items.ItemInstances?.Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber)).Select(x => x.SerialNumber).ToList() ?? new List<string>()
                      
                    )).ToList() ?? new List<SalesItemsDto>()
                   
                )).ToList();
                    
                }



                var totalItems = filterSales.Count();

                var paginatedJournalEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? filterSales
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : filterSales.ToList();

                var pagedResult = new PagedResult<GetAllSalesDetailsByQueryResponse>
                {
                    Items = paginatedJournalEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllSalesDetailsByQueryResponse>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching all sales details", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesItemsByQueryResponse>>> GetAllSalesItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesItems = await _unitOfWork.BaseRepository<SalesItems>().GetAllAsyncWithPagination();
                var salesItemsPagedResult = await salesItems.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allSalesItemsDisplay = _mapper.Map<PagedResult<GetAllSalesItemsByQueryResponse>>(salesItemsPagedResult.Data);

                return Result<PagedResult<GetAllSalesItemsByQueryResponse>>.Success(allSalesItemsDisplay);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Sales Return Items", ex);
            }
        }

        public async Task<Result<BIllNumberGenerationByCompanyQueryResponse>> GetBillNumberStatusByCompany(string id, CancellationToken cancellationToken)
        {
            try
            {
                var companyDetails = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(id);

                if (companyDetails == null)
                {
                    return Result<BIllNumberGenerationByCompanyQueryResponse>.Failure("Company not found.");
                }

                var response = new BIllNumberGenerationByCompanyQueryResponse(
                    companyDetails.BillNumberGenerationTypeForSales,
                    id
                );

                return Result<BIllNumberGenerationByCompanyQueryResponse>.Success(response);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured whilw Getting Status by Company{id}");
            }
        }

        public async Task<Result<string>> GetCurrentSalesBillNumber()
        {
            string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;

            var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);

            var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);
            if (company == null)
            {
                return Result<string>.Failure("Invalid CompanyId. Company does not exist.");
            }
            var billNumber = "";
            if (company.BillNumberGenerationTypeForSales == BillNumberGenerationType.Manual)
            {
                return Result<string>.Failure("Billnumber for Sales is manual");
            }
            else
            {
                billNumber = await _billNumberGenerator.GenerateBillNumberAsync(companyId, "sales", fiscalYear.Data.fyName);
            }
            ;

            if (string.IsNullOrEmpty(billNumber))
            {
                return Result<string>.Failure("Failed to generate bill number. Please check company settings.");
            }

            return Result<string>.Success(billNumber);
       
        }

        public async Task<Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>> GetFilterSalesDetails(PaginationRequest request,FilterSalesDetailsDTOs filterSalesDetailsDTOs)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SalesDetails>();

                var filterItems = isSuperAdmin
                    ? ledger
                    : ledger.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                // Parse provided start date
                if (!string.IsNullOrWhiteSpace(filterSalesDetailsDTOs.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDetailsDTOs.startDate.Trim());

                    if (DateTime.TryParse(filterSalesDetailsDTOs.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                // Parse provided end date
                if (!string.IsNullOrWhiteSpace(filterSalesDetailsDTOs.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDetailsDTOs.endDate.Trim());

                    if (DateTime.TryParse(filterSalesDetailsDTOs.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // If both dates missing, default to full today
                bool isStartInvalid = !startDateUtc.HasValue || startDateUtc == DateTime.MinValue;
                bool isEndInvalid = !endDateUtc.HasValue || endDateUtc == DateTime.MinValue;

                if (isStartInvalid && isEndInvalid)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (!isStartInvalid && isEndInvalid)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (isStartInvalid && !isEndInvalid)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                // Get current logged-in user
                var userId = _tokenService.GetUserId;

                // Fetch filtered sales
                var salesDetailsResult = await _unitOfWork.BaseRepository<SalesDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            //x.CreatedBy == userId() &&
                            x.IsActive == true &&
                            (string.IsNullOrEmpty(filterSalesDetailsDTOs.ledgerId) || x.LedgerId.ToLower().Contains(filterSalesDetailsDTOs.ledgerId.ToLower())) &&
                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc) && x.Status != SalesStatus.Returned,
                        queryModifier: q => q
                            .Include(sd => sd.SalesItems.Where(pi => pi.IsActive))
                                .ThenInclude(i => i.Item)
                            .Include(sd => sd.SalesItems.Where(pi => pi.IsActive))
                                .ThenInclude(i => i.ItemInstances)
                    );

                // Map to response
                var responseList = salesDetailsResult.Select(sd =>
                {
                    decimal subTotalAmount = sd.SalesItems?.Sum(i => i.Amount) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - sd.DiscountAmount;

                    return new FilterSalesDetailsByDateQueryResponse(
                       sd.Id,
                       sd.Date,
                       sd.BillNumber,
                       sd.LedgerId,
                       sd.AmountInWords,
                       sd.DiscountPercent,
                       sd.DiscountAmount,
                       sd.VatPercent,
                       sd.VatAmount,
                       sd.CompanyId,
                       sd.Status,
                       sd.GrandTotalAmount,
                       sd.PaymentId,
                       sd.StockCenterId,
                         taxableAmount,
                        subTotalAmount,
                       sd.SalesItems?.Select(i => new SalesItemsDto(
                           i.Id,
                           i.Quantity,
                           i.UnitId,
                           i.ItemId,
                           i.Price,
                           i.Amount,
                           i.CreatedBy,
                           i.CreatedAt,
                           i.UpdatedBy,
                           i.UpdatedAt,
                           i.Item?.HsCode ?? "",
                           i.Item.IsVatEnables,
                           i.ItemInstances?.Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber)).Select(x => x.SerialNumber).ToList() ?? new List<string>()
                       )).ToList() ?? new List<SalesItemsDto>(),
                       sd.SalesItems?.Select(y => new QuantityDetailDto(
                           y.Id,
                           y.ItemId,
                           y.UnitId,
                           y.Quantity,
                           y.ItemInstances?.Where(s => !string.IsNullOrWhiteSpace(s.SerialNumber))
                                           .Select(s => s.SerialNumber).ToList() ?? new List<string?>()
                       )).ToList() ?? new List<QuantityDetailDto>()
                   );
                }).ToList();

                PagedResult<FilterSalesDetailsByDateQueryResponse> finalResponseList;

                if (request.IsPagination)
                {

                    int pageIndex = request.pageIndex <= 0 ? 1 : request.pageIndex;
                    int pageSize = request.pageSize <= 0 ? 10 : request.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSalesDetailsByDateQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSalesDetailsByDateQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching sales details: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterSalesQuotationQueryResponse>>> GetFilterSalesQuotation(PaginationRequest paginationRequest, FilterSalesDetailsDTOs filterSalesDetailsDTOs)
        {
            try
            {
                var (salesQuotation, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SalesQuotationDetails>();

                var filterItems = isSuperAdmin
                    ? salesQuotation
                    : salesQuotation.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || x.CompanyId == "");

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                // Convert start and end dates
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterSalesDetailsDTOs.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDetailsDTOs.startDate.Trim());
                    if (DateTime.TryParse(filterSalesDetailsDTOs.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filterSalesDetailsDTOs.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(filterSalesDetailsDTOs.endDate.Trim());
                    if (DateTime.TryParse(filterSalesDetailsDTOs.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // Default to today if dates are invalid
                if (!startDateUtc.HasValue && !endDateUtc.HasValue)
                {
                    var today = DateTime.UtcNow.Date;
                    startDateUtc = today;
                    endDateUtc = today.AddDays(1);
                }
                else if (startDateUtc.HasValue && !endDateUtc.HasValue)
                {
                    endDateUtc = startDateUtc.Value.AddDays(1);
                }
                else if (!startDateUtc.HasValue && endDateUtc.HasValue)
                {
                    startDateUtc = endDateUtc.Value.AddDays(-1);
                }

                var userId = _tokenService.GetUserId;

                var salesDetailsResult = await _unitOfWork.BaseRepository<SalesQuotationDetails>()
                    .GetConditionalAsync(
                        predicate: s =>
                            s.CreatedBy == userId() &&
                            s.IsActive == true &&
                            (string.IsNullOrEmpty(filterSalesDetailsDTOs.ledgerId) ||
                             EF.Functions.Like(s.LedgerId, $"%{filterSalesDetailsDTOs.ledgerId}%")) &&
                            (startDateUtc == null || s.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || s.CreatedAt <= endDateUtc),
                        queryModifier: q => q
                            .Include(sd => sd.SalesQuotationItems)


                    );

                // Optional: filter inactive sales items in-memory (if needed)
                var salesDetailsList = salesDetailsResult.ToList();

                salesDetailsList.ForEach(sd =>
                {
                    sd.SalesQuotationItems = sd.SalesQuotationItems
                        .Where(pi => pi.IsActive)
                        .ToList();
                });



                var responseList = salesDetailsResult.Select(sd =>
                {

                    decimal subTotalAmount = sd.SalesQuotationItems?.Sum(i => i.Amount) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - sd.DiscountAmount;
                    return new FilterSalesQuotationQueryResponse(
                          sd.Id,
                          sd.Date,
                          sd.QuotationNumber,
                          sd.LedgerId,
                          sd.AmountInWords,
                          sd.DiscountPercent,
                          sd.DiscountAmount,
                          sd.VatPercent,
                          sd.VatAmount,
                          sd.CompanyId,
                          sd.GrandTotalAmount,
                          sd.StockCenterId,
                          sd.QuotationStatuss,
                                                       taxableAmount,
subTotalAmount,
                          sd.SalesQuotationItems?.Select(i => new SalesQuotationItemsDto(
                              i.Id,
                              i.Quantity,
                              i.UnitId,
                              i.ItemId,
                              i.Price,
                              i.Amount,
                              i.CreatedBy,
                              i.CreatedAt,
                              i.UpdatedBy,
                              i.UpdatedAt,
                              i.SalesQuotationDetailsId,
                              i.IsActive
                          )).ToList() ?? new List<SalesQuotationItemsDto>()
                      );
                }).ToList();

                PagedResult<FilterSalesQuotationQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterSalesQuotationQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterSalesQuotationQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<FilterSalesQuotationQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching salesDetails: {ex.Message}", ex);
            }
        }

        public async Task<Result<GetSalesDetailsByIdQueryResponse>> GetSalesDetailsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>().
                    GetConditionalAsync(x => x.Id == id && x.IsActive == true,
                    query => query.Include(rm => rm.SalesItems).ThenInclude(x => x.Item).ThenInclude(x=>x.ItemInstances));

                var salesPaymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
           .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Sales);

                var sales = salesDetails.FirstOrDefault();

                if (sales == null)
                {
                    return null;
                }



                var billSundryDetails = await _unitOfWork.BaseRepository<BillSundryDetails>().GetConditionalAsync(x => x.PurchaseSalesAndReturnId == sales.Id);






                var itemIdsInSales = sales.SalesItems
                    .Select(si => si.ItemId)
                    .Distinct()
                    .ToList();

                var filteredInventory = await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(x => itemIdsInSales.Contains(x.ItemId));

                var inventorySummary = filteredInventory
                  .GroupBy(inv => inv.ItemId)
                  .ToDictionary(
                      g => g.Key,
                      g => g.Sum(x => x.QuantityOut)
                  );






                var salesDetailsResponse = new GetSalesDetailsByIdQueryResponse(
                    sales.Id,
                    sales.Date,
                    sales.BillNumber,
                    sales.LedgerId,
                    sales.AmountInWords,
                    sales.DiscountPercent ?? 0,
                    sales.DiscountAmount ?? 0,
                    sales.VatPercent ?? 0,
                    sales.VatAmount ?? 0,
                    sales.CompanyId,
                    sales.GrandTotalAmount,
                    sales.PaymentId,
                    sales.StockCenterId,
                    (salesPaymentDetails as ChequePayment)?.ChequeNumber,
                    (salesPaymentDetails as ChequePayment)?.BankName,
                   (salesPaymentDetails as ChequePayment)?.AccountName,
                   sales.SubTotalAmount,
                   sales.TaxableAmount,
                   sales.AmountAfterVat,
                   billSundryDetails?.Select(x =>
        new BillSundryRequestDTOs(
            x.BillSundryId,
            x.Rate ?? 0
        )
    ).ToList() ?? new List<BillSundryRequestDTOs>(),


                    sales.SalesItems?.Select(detail =>
                    {
                        var totalQtyOut = inventorySummary != null && inventorySummary.ContainsKey(detail.ItemId)
                            ? inventorySummary[detail.ItemId]
                            : 0;

                        return new SalesItemsDto(
                            detail.Id,
                            detail.Quantity,
                            detail.UnitId,
                            detail.ItemId,
                            detail.Price,
                            detail.Amount,
                            detail.CreatedBy,
                            detail.CreatedAt,
                            detail.UpdatedBy,
                            detail.UpdatedAt,
                            detail.Item?.HsCode ?? "", // safe
                            detail.Item.IsVatEnables,
                            detail.ItemInstances?
                                .Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                                .Select(x => x.SerialNumber)
                                .ToList() ?? new List<string>()
                            
                        );
                    }).ToList() ?? new List<SalesItemsDto>()
                );


                return Result<GetSalesDetailsByIdQueryResponse>.Success(salesDetailsResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales details by id", ex);

            }
        }

        public async Task<Result<GetSalesDetailsQueryResponse>> GetSalesDetailsByRefNo(string referenceNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                string decodedReferenceNumber = WebUtility.UrlDecode(referenceNumber);

                var salesDetail = await _unitOfWork.BaseRepository<SalesDetails>()
                .GetConditionalAsync(
                    x => x.BillNumber.Trim() == decodedReferenceNumber.Trim() && x.IsActive == true,
                    query => query.Include(rm => rm.SalesItems)
                                  .ThenInclude(x => x.ItemInstances)
                );
                var sales = salesDetail.FirstOrDefault();

                if (sales == null)
                {
                    return null;
                }

                var itemIdsInSales = sales.SalesItems
                    .Select(si => si.ItemId)
                    .Distinct()
                    .ToList();

                var filteredInventory = await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(x => itemIdsInSales.Contains(x.ItemId));

                var inventorySummary = filteredInventory
                  .GroupBy(inv => inv.ItemId)
                  .ToDictionary(
                      g => g.Key,
                      g => g.Sum(x => x.QuantityOut)
                  );

                var salesResponse = new GetSalesDetailsQueryResponse(
                    sales.Id,
                    sales.Date,
                    sales.BillNumber,
                    sales.LedgerId,
                    sales.AmountInWords,
                    sales.DiscountPercent ?? 0,
                    sales.DiscountAmount ?? 0,
                    sales.VatPercent ?? 0,
                    sales.VatAmount ?? 0,
                    sales.CompanyId,
                    sales.GrandTotalAmount,
                    "",
                    sales.PaymentId,
                    sales.StockCenterId,
                    sales.SalesItems?.Select(detail =>
                    {
                        var totalQtyOut = inventorySummary != null && inventorySummary.ContainsKey(detail.ItemId)
                            ? inventorySummary[detail.ItemId]
                            : 0;

                        return new SalesItemsDto(
                            detail.Id,
                            detail.Quantity,
                            detail.UnitId,
                            detail.ItemId,
                            detail.Price,
                            detail.Amount,
                            detail.CreatedBy,
                            detail.CreatedAt,
                            detail.UpdatedBy,
                            detail.UpdatedAt,
                            detail.Item?.HsCode ?? "",
                            detail.Item.IsVatEnables,
                            detail.ItemInstances?
                                .Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                                .Select(x => x.SerialNumber)
                                .ToList() ?? new List<string>()
                           
                        );
                    }).ToList() ?? new List<SalesItemsDto>()
                );


                return Result<GetSalesDetailsQueryResponse>.Success(salesResponse);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales details by referenceNo", ex);

            }
        }

        public async Task<Result<GetSalesItemsDetailsByItemIdQueryResponse>> GetSalesDetailsItems(string itemsId, CancellationToken cancellationToken = default)
        {
            try
            {

                var companyId = _tokenService.CompanyId().FirstOrDefault();

                var salesDetails = await _unitOfWork.BaseRepository<Inventories>()
                    .GetConditionalAsync(x => x.ItemId == itemsId && x.companyId == companyId,
                        query => query
                            .Include(s => s.Items)
                 
                    );

                if (salesDetails == null || !salesDetails.Any())
                {
                    return Result<GetSalesItemsDetailsByItemIdQueryResponse>.Failure("No sales details found.");
                }

                var purchaseItemIds = salesDetails
                    .Where(x => !string.IsNullOrEmpty(x.PurchaseItemsId))
                    .Select(x => x.PurchaseItemsId)
                    .Distinct()
                    .ToList();


                var salesItemsIdList = salesDetails
                    .Where(x => !string.IsNullOrEmpty(x.SalesItemsId))
                    .Select(x => x.SalesItemsId)
                    .Distinct()
                    .ToList();

                var itemInstances = await _unitOfWork.BaseRepository<ItemInstance>()
                    .GetConditionalAsync(x =>
                        x.ItemsId == itemsId &&
                        (purchaseItemIds.Contains(x.PurchaseItemsId) || x.PurchaseItemsId == null) &&
                        (x.SalesItemsId == null || !salesItemsIdList.Contains(x.SalesItemsId)));



                var serialNumbers = itemInstances
                   .Where(x => !string.IsNullOrEmpty(x.SerialNumber))
                   .Select(x => x.SerialNumber!)
                   .ToList();
                var groupedSales = salesDetails
                    .GroupBy(x => new { x.ItemId, x.companyId })
                    .Select(g =>
                    {
                        var firstItem = g.First();

                        // Safe parsing of SellingPrice
                        decimal sellingPrice = 0m;
                        var sellingPriceStr = firstItem.Items.SellingPrice;
                        if (!string.IsNullOrWhiteSpace(sellingPriceStr))
                            decimal.TryParse(sellingPriceStr, out sellingPrice);

                        // Sum AmountIn and QuantityIn safely
                        decimal totalAmount = g.Sum(x => x.AmountIn);        // if nullable, use x.AmountIn ?? 0
                        decimal totalQuantity = g.Sum(x => x.QuantityIn);

                        bool? isVatEnabled = firstItem.Items.IsVatEnables;
                        bool? isConversionFactor = firstItem.Items.IsConversionFactor;
                        string? conversionFactorId = firstItem.Items.ConversionFactorId;

                        return new GetSalesItemsDetailsByItemIdQueryResponse(
                            firstItem.UnitId,
                            totalQuantity,
                            sellingPrice,
                            isVatEnabled,
                            isConversionFactor,
                            conversionFactorId,
                            serialNumbers
                        );
                    })
                    .FirstOrDefault();



                return Result<GetSalesItemsDetailsByItemIdQueryResponse>.Success(groupedSales);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales details by id", ex);

            }
        }

        public async Task<Result<GetSalesQuotationByIdQueryResponse>> GetSalesQuotationById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesQuotationDetails = await _unitOfWork.BaseRepository<SalesQuotationDetails>().
                    GetConditionalAsync(x => x.Id == id && x.IsActive == true,
                    query => query.Include(rm => rm.SalesQuotationItems));

                var sales = salesQuotationDetails.FirstOrDefault();

                if (sales == null)
                {
                    return null;
                }

                var response = new GetSalesQuotationByIdQueryResponse(
                   sales.Id,
                   sales.Date,
                     sales.QuotationNumber,
                     sales.LedgerId,
                        sales.AmountInWords,
                        sales.DiscountPercent ?? 0,
                        sales.DiscountAmount ?? 0,
                        sales.VatPercent ?? 0,
                        sales.VatAmount ?? 0,
                        sales.CompanyId,
                        sales.GrandTotalAmount,
                        sales.StockCenterId,


                    sales.SalesQuotationItems?.Select(detail =>
                    {
                        return new SalesQuotationItemsDto(
                            detail.Id,
                            detail.Quantity,
                            detail.UnitId,
                            detail.ItemId,
                            detail.Price,
                            detail.Amount,
                            detail.CreatedBy,
                            detail.CreatedAt,
                            detail.UpdatedBy,
                            detail.UpdatedAt,
                            detail.SalesQuotationDetailsId,
                           detail.IsActive

                        );
                    }).ToList() ?? new List<SalesQuotationItemsDto>()
                );


                return Result<GetSalesQuotationByIdQueryResponse>.Success(response);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales Quotation by id", ex);

            }
        }

        public async Task<Result<QuotationToSalesResponse>> QuotationToSales(QuotationToSalesCommand quotationToSalesCommand)
        {
          
                if (string.IsNullOrEmpty(quotationToSalesCommand.salesQuotationId))
                {
                    return Result<QuotationToSalesResponse>.Failure("InvalidRequest", "Sales Quotation ID cannot be null or empty");
                }

                var userId = _tokenService.GetUserId();
                var companyId = _tokenService.CompanyId().FirstOrDefault();
                var FyId = _fiscalContext.CurrentFiscalYearId;
                var newSalesId = Guid.NewGuid().ToString();

                var salesDetails = await _unitOfWork.BaseRepository<SalesQuotationDetails>()
                    .GetConditionalAsync(x => x.Id == quotationToSalesCommand.salesQuotationId && x.IsActive == true,
                        q => q.Include(rm => rm.SalesQuotationItems).ThenInclude(Ii=>Ii.ItemInstances));



                var salesQuotation = salesDetails.FirstOrDefault();

                
                if (salesQuotation == null)         
                {
                    return Result<QuotationToSalesResponse>.Failure("NotFound", "Sales Quotation not found");
                }

                var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);
                if (company == null)
                {
                    return Result<QuotationToSalesResponse>.Failure("Invalid CompanyId. Company does not exist.");
                }


                var mappedSalesQuotation = new AddSalesDetailsCommand(

                     date: salesQuotation.Date,
                     billNumber: quotationToSalesCommand.billNumbers,
                     ledgerId: salesQuotation.LedgerId,
                     amountInWords: salesQuotation.AmountInWords,
                     discountPercent: salesQuotation.DiscountPercent,
                     discountAmount: salesQuotation.DiscountAmount,
                     vatPercent: salesQuotation.VatPercent,
                     vatAmount: salesQuotation.VatAmount,
                     grandTotalAmount: salesQuotation.GrandTotalAmount,
                     paymentId: quotationToSalesCommand.paymentId,
                     isSales: true, // you must decide the value here
                     StockCenterId: salesQuotation.StockCenterId,
                     chequeNumber : quotationToSalesCommand.chequeNumber,
                     bankName: quotationToSalesCommand.bankName,
                     accountName: quotationToSalesCommand.accountName,
                     salesQuotationNumber: salesQuotation.SalesQuotationNumber,
                        subTotalAmount: salesQuotation.SubTotalAmount,
                        taxableAmount: salesQuotation.TaxableAmount,
                        amountAfterVat: salesQuotation.AmountAfterVat,
                     SalesItems: salesQuotation.SalesQuotationItems.Select(x =>
                         new AddSalesItemsRequest(
                             x.Quantity,
                             x.UnitId,
                             x.ItemId,
                             x.Price,
                             x.Amount,
                             x.ItemInstances.Select(x=>x.SerialNumber).ToList() ?? new List<string>()
                             //x.ItemInstances.ToList()
                         )
                     ).ToList()
                 );



                var addSalesDetails = await Add(mappedSalesQuotation);


                var resultDTOs = new QuotationToSalesResponse(
                  
                    addSalesDetails.Data.Id,
                    addSalesDetails.Data.Date,
                    addSalesDetails.Data.BillNumber,
                    addSalesDetails.Data.LedgerId,
                    addSalesDetails.Data.AmountInWords,
                    addSalesDetails.Data.DiscountPercent,
                    addSalesDetails.Data.DiscountAmount,
                    addSalesDetails.Data.VatPercent,
                    addSalesDetails.Data.VatAmount,
                    addSalesDetails.Data.CreatedBy,
                    addSalesDetails.Data.CompanyId,
                    addSalesDetails.Data.CreatedAt,
                    addSalesDetails.Data.GrandTotalAmount,
                    addSalesDetails.Data.PaymentId,
                    addSalesDetails.Data.StockCenterId,
                    addSalesDetails.Data.SalesItems.Select(x =>
                        new AddSalesItemsRequest(
                            x.quantity,
                            x.unitId,
                            x.itemId,
                            x.price,
                            x.amount,
                            x.serialNumbers?.ToList() ?? new List<string>()
                        )
                    ).ToList()
                );

                return Result<QuotationToSalesResponse>.Success(resultDTOs);

        }

        public async Task<Result<UpdateSalesDetailsResponse>> Update(string id, UpdateSalesDetailsCommand updateSalesDetailsCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(id))
                        return Result<UpdateSalesDetailsResponse>.Failure("InvalidRequest", "Sales ID cannot be null or empty");

                    var userId = _tokenService.GetUserId();
                    var companyId = _tokenService.CompanyId().FirstOrDefault();
                    var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
                        .GetConditionalAsync(x => x.Id == id && x.IsActive == true, q => q.Include(rm => rm.SalesItems).ThenInclude(si => si.ItemInstances));
                    var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);
                    if (company == null)
                    {
                        return Result<UpdateSalesDetailsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }

  
               
                    var sales = salesDetails.FirstOrDefault();
                    if (sales == null)
                        return Result<UpdateSalesDetailsResponse>.Failure("NotFound", "sales details not found");

                    sales.CompanyId = companyId;
                    sales.ModifiedBy = userId;
                    sales.ModifiedAt = DateTime.Now;



                    // === Update SalesItems and Serial Numbers ===
                    if (updateSalesDetailsCommand.updateSalesItems != null && updateSalesDetailsCommand.updateSalesItems.Any())
                    {
                        var existingItems = sales.SalesItems.ToList();

                        foreach (var detail in updateSalesDetailsCommand.updateSalesItems)
                        {
                            var existingItem = existingItems.FirstOrDefault(x => x.Id == detail.id);

                            if (existingItem != null)
                            {
                                _mapper.Map(detail, existingItem);

                                var itemInstances = existingItem.ItemInstances?.ToList() ?? new List<ItemInstance>();

                                for (int i = 0; i < detail.serialNumbers?.Count; i++)
                                {
                                    var serial = detail.serialNumbers[i];

                                    if (string.IsNullOrWhiteSpace(serial))
                                        continue;


                                    if (i < itemInstances.Count)
                                    {
                                        itemInstances[i].SerialNumber = detail.serialNumbers[i];
                                    }
                                    else
                                    {
                                        itemInstances.Add(new ItemInstance
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            SalesItemsId = existingItem.Id,
                                            ItemsId = existingItem.ItemId,
                                            SerialNumber = detail.serialNumbers[i],
                                            Date = DateTime.UtcNow
                                        });
                                    }
                                }

                                itemInstances = itemInstances.Take(detail.serialNumbers.Count).ToList();
                                existingItem.ItemInstances = itemInstances;

                                _unitOfWork.BaseRepository<SalesItems>().Update(existingItem);
                            }
                            else
                            {
                                var newItem = _mapper.Map<SalesItems>(detail);
                                newItem.Id = Guid.NewGuid().ToString();
                                newItem.SalesDetailsId = sales.Id;
                                newItem.CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                                newItem.UpdatedAt = newItem.CreatedAt;
                                newItem.CreatedBy = userId;
                                newItem.UpdatedBy = "";
                                sales.SalesItems.Add(newItem);
                            }
                        }

                        var updatedItemIds = updateSalesDetailsCommand.updateSalesItems.Select(x => x.id).ToList();
                        var itemsToRemove = existingItems.Where(x => !updatedItemIds.Contains(x.Id)).ToList();
                        foreach (var itemToRemove in itemsToRemove)
                        {
                            sales.SalesItems.Remove(itemToRemove);
                            _unitOfWork.BaseRepository<SalesItems>().Delete(itemToRemove);
                        }
                    }

                    // === Save Sales First ===
                    _mapper.Map(updateSalesDetailsCommand, sales);
                    await _unitOfWork.SaveChangesAsync();


                    #region Update Sales with Bill Sundry, VAT, and Discount

                    // Fetch existing Journal Entry
                    var journal = (await _unitOfWork.BaseRepository<JournalEntry>()
                        .GetConditionalAsync(j => j.Id == sales.JournalEntriesId,
                            q => q.Include(j => j.JournalEntryDetails)))
                        .FirstOrDefault();

                    if (journal == null)
                        throw new Exception("Journal entry not found");

                    // Remove old journal details
                    _unitOfWork.BaseRepository<JournalEntryDetails>().DeleteRange(journal.JournalEntryDetails.ToList());

                    // Prepare totals
                    decimal gross = sales.SalesItems.Sum(x => x.Amount);
                    decimal discount = sales.DiscountAmount ?? 0;
                    decimal vat = sales.VatAmount ?? 0;
                    decimal netAmount = gross - discount + vat;

                    DateTime transactionDate = DateTime.TryParse(sales.Date, out var dt) ? dt : sales.CreatedAt;
                    string fiscalId = journal.FyId ?? _fiscalContext.CurrentFiscalYearId;
                    var journalDetails = new List<JournalEntryDetails>();

                    // ------------------- BILL SUNDRY -------------------
                    decimal billSundrySalesTotal = 0;
                    decimal billSundryCustomerTotal = 0;


                    if (updateSalesDetailsCommand.BillSundryIds?.Any() == true)
                    {
                        var billSundryDetailsList = new List<BillSundryDetails>();

                        foreach (var bsId in updateSalesDetailsCommand.BillSundryIds.Where(x => !string.IsNullOrEmpty(x.billSundryIds)))
                        {
                            var calcDto = new CalculationBIllDTOs(
                                bsId.billSundryIds,
                                gross,
                                gross, // taxableAmount (if different, adjust accordingly)
                                netAmount
                            );

                            var bsConfig = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(bsId.billSundryIds);
                            bsConfig.DefaultValue = bsId.rate ?? 0;
                            _unitOfWork.BaseRepository<BillSundry>().Update(bsConfig);

                            var billSundryDetails = new BillSundryDetails
                            {
                                Id = Guid.NewGuid().ToString(),
                                BillSundryId = bsId.billSundryIds,
                                Rate = bsConfig.DefaultValue ?? 0,
                                PurchaseSalesAndReturnId = sales.Id
                            };
                            billSundryDetailsList.Add(billSundryDetails);

                            // Calculate
                            var calcResult = await _billSundryServices.CalculateBillSundry(calcDto);
                            if (!calcResult.IsSuccess)
                                return Result<UpdateSalesDetailsResponse>.Failure("Bill Sundry Calculation Error");

                            var bsResponse = calcResult.Data;



                            if (bsConfig.IsSalesAccountingAffected)
                            {
                                // Credit Sales Ledger
                                decimal creditAmount = !bsConfig.IsSalesAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                decimal debitAmount = !bsConfig.CustomerAmountAdjusted ? bsResponse.CalculatedAmount : 0;

                                if (creditAmount > 0)
                                {
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                         journal.Id,
                                        bsConfig.SalesAdjustedLedgerId,
                                        0,
                                        creditAmount,
                                        transactionDate,
                                        companyId,
                                         fiscalId,
                                         true
                                    ));
                                    billSundrySalesTotal += creditAmount;
                                }

                                // Debit Customer Ledger
                                if (debitAmount > 0)
                                {
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                         journal.Id,
                                        bsConfig.CustomerAdjustedLedgerId,
                                        debitAmount,
                                        0,
                                        transactionDate,
                                        companyId,
                                         fiscalId,
                                         true
                                    ));
                                    billSundryCustomerTotal += debitAmount;
                                }
                            }

                        }

                        await _unitOfWork.BaseRepository<BillSundryDetails>().AddRange(billSundryDetailsList);
                    }

                    // ------------------- SALES / VAT / DISCOUNT -------------------
                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        journal.Id,
                        LedgerConstants.SalesLedgerId,
                        0,
                        gross,
                        transactionDate,
                        sales.CompanyId,
                        fiscalId,
                        true
                    ));

                    if (vat > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            journal.Id,
                            LedgerConstants.VATLedgerId,
                            0,
                            vat,
                            transactionDate,
                            sales.CompanyId,
                            fiscalId,
                            true
                        ));
                    }

                    if (discount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            journal.Id,
                            LedgerConstants.DiscountLedgerId,
                            discount,
                            0,
                            transactionDate,
                            sales.CompanyId,
                            fiscalId,
                            true
                        ));
                    }

                    // ------------------- PAYMENT / CUSTOMER LEDGER -------------------
                    string ledgerIdToUse = null;
                    var hasLedger = !string.IsNullOrEmpty(updateSalesDetailsCommand.ledgerId);
                    var hasPayment = !string.IsNullOrEmpty(updateSalesDetailsCommand.paymentId);
                    var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                    var isSpecial = updateSalesDetailsCommand.paymentId == specialPaymentId;

                    void AddJournal(string ledgerId, decimal debit, decimal credit)
                    {
                        if (string.IsNullOrEmpty(ledgerId))
                            throw new ArgumentException("Ledger Id cannot be null or empty");

                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            journal.Id,
                            ledgerId,
                            debit,
                            credit,
                            transactionDate,
                            sales.CompanyId,
                            fiscalId,
                            true
                        ));
                    }

                    if (isSpecial && hasLedger)
                    {
                        AddJournal(updateSalesDetailsCommand.ledgerId, netAmount + billSundryCustomerTotal, 0);
                    }
                    else if (hasPayment)
                    {
                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>()
                            .GetByGuIdAsync(updateSalesDetailsCommand.paymentId);

                        if (paymentMethod == null)
                            throw new Exception("Payment Method not found");

                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                            ?? throw new Exception("Ledger Group not found");

                        var ledger = await _unitOfWork.BaseRepository<Ledger>()
                            .FirstOrDefaultAsync(x =>x.IsActive && x.SubLedgerGroupId == subledgerGroup.Id);

                        ledgerIdToUse = subledgerGroup.Id switch
                        {
                            SubLedgerGroupConstants.CashInHands => ledger?.Id ?? LedgerConstants.CashLedgerId,
                            SubLedgerGroupConstants.BankAccounts => ledger?.Id ?? LedgerConstants.BankLedgerId,
                            _ => ledger?.Id ?? updateSalesDetailsCommand.ledgerId
                        };

                        AddJournal(ledgerIdToUse, netAmount + billSundryCustomerTotal, 0);
                    }
                    else if (hasLedger)
                    {
                        AddJournal(updateSalesDetailsCommand.ledgerId, netAmount + billSundryCustomerTotal, 0);
                    }

                    // ------------------- BALANCE CHECK -------------------
                    if (journalDetails.Sum(x => x.DebitAmount) != journalDetails.Sum(x => x.CreditAmount))
                        throw new InvalidOperationException("Updated Sales journal is unbalanced.");

                    // ------------------- SAVE UPDATED JOURNAL -------------------
                    await _unitOfWork.BaseRepository<JournalEntryDetails>().AddRange(journalDetails);

                    journal.TransactionDate = transactionDate;
                    journal.Description = "Updated Sales Entry with Bill Sundry, VAT, Discount";
                    journal.ModifiedAt = DateTime.UtcNow;
                    journal.ModifiedBy = userId;

                    _unitOfWork.BaseRepository<JournalEntry>().Update(journal);
                    await _unitOfWork.SaveChangesAsync();

                    #endregion


                    #region Update PaymentDetails

                    var paymentDetails = await _unitOfWork.BaseRepository<PaymentsDetails>()
                  .FirstOrDefaultAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.Sales);

                    if (paymentDetails is not null)
                    {
                        paymentDetails.TotalAmount = Convert.ToDecimal(updateSalesDetailsCommand.grandTotalAmount);
                        _unitOfWork.BaseRepository<PaymentsDetails>().Update(paymentDetails);
                    }


                    #endregion


                    scope.Complete();

                    // === Prepare Response ===
                    var resultResponse = new UpdateSalesDetailsResponse
                    (
                        id,
                        sales.Date,
                        sales.BillNumber,
                        sales.LedgerId,
                        sales.AmountInWords,
                        sales.DiscountPercent ?? 0,
                        sales.DiscountAmount ?? 0,
                        sales.VatPercent ?? 0,
                        sales.VatAmount ?? 0,
                        sales.GrandTotalAmount,
                        sales.PaymentId,
                        sales.StockCenterId,
                        sales.SalesItems?.Select(detail => new UpdateSalesItems(
                            detail.Quantity,
                            detail.UnitId,
                            detail.ItemId,
                            detail.Price,
                            detail.Amount,
                            detail.SalesDetailsId
                        )).ToList() ?? new List<UpdateSalesItems>()
                    );

                    return Result<UpdateSalesDetailsResponse>.Success(resultResponse);
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception($"An error occurred while updating the SalesDetails by {id}", ex);
                }


            }
        }

        public async Task<Result<UpdateBillNumberGenerationByCompanyResponse>> UpdateBillNumberStatusByCompany(UpdateBillNumberGenerationByCompanyCommand updateBillNumberGenerationByCompanyCommand)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (string.IsNullOrEmpty(updateBillNumberGenerationByCompanyCommand.companyId))
                    {
                        return Result<UpdateBillNumberGenerationByCompanyResponse>.Failure("InvalidRequest", "Company ID cannot be null or empty");
                    }

                    var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(updateBillNumberGenerationByCompanyCommand.companyId);

                    company.BillNumberGenerationTypeForSales = updateBillNumberGenerationByCompanyCommand.BillNumberGenerationType;
                    _unitOfWork.BaseRepository<Company>().Update(company);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateBillNumberGenerationByCompanyResponse
                    (
                            company.BillNumberGenerationTypeForSales,
                            company.Id

                    );

                    return Result<UpdateBillNumberGenerationByCompanyResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the bill number generator for sales Details", ex);
                }


            }
        }
    }
}
