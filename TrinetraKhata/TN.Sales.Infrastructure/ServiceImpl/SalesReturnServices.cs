using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TN.Account.Application.ServiceInterface;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Authentication.Domain.Static.Roles;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails;
using TN.Sales.Application.SalesReturn.Queries;
using TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate;
using TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems;
using TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById;
using TN.Sales.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.CalculationBillSundry;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Account;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Domain.Static.Cache;
using static TN.Authentication.Domain.Entities.CompanySettings;
using static TN.Sales.Domain.Entities.SalesDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Sales.Infrastructure.ServiceImpl
{
    public class SalesReturnServices : ISalesReturnServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IDateConvertHelper _dateConverterHelper;
        private readonly IBillNumberGenerator _billNumberGenerator;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IMediator _mediator;
        private readonly FiscalContext _fiscalContext;
        private readonly ISettingServices _settingServices;
        private readonly IBillSundryServices _billSundryServices;
        public SalesReturnServices(IBillSundryServices billSundryServices,IMediator mediator,ISettingServices settingServices, IGetUserScopedData getUserScopedData, FiscalContext fiscalContext, IBillNumberGenerator billNumberGenerator, IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _billNumberGenerator = billNumberGenerator;
            _getUserScopedData = getUserScopedData;
            _tokenService = tokenService;
            _dateConverterHelper = dateConvertHelper;
            _unitOfWork = unitOfWork;
            _fiscalContext = fiscalContext;
            _mapper = mapper;
            _mediator = mediator;
            _settingServices = settingServices;
            _billSundryServices = billSundryServices;
        }

        public async Task<Result<AddSalesReturnDetailsResponse>> Add(AddSalesReturnDetailsCommand request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var FyId = _fiscalContext.CurrentFiscalYearId;
                    string newId = Guid.NewGuid().ToString();
                    string userId = _tokenService.GetUserId();
                    string companyId = _tokenService.CompanyId().FirstOrDefault() ?? string.Empty;

                    var company = await _unitOfWork.BaseRepository<Company>().GetByGuIdAsync(companyId);
                    if (company == null)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }

                    string nepaliDate = request.returnDate
                     ?? (await _dateConverterHelper.ConvertToNepali(DateTime.Today));


                    DateTime returnedDate = request.returnDate == default
                  ? DateTime.Today
                  : await _dateConverterHelper.ConvertToEnglish(request.returnDate);



                    var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
                      .GetConditionalAsync(x => x.Id == request.salesDetailsId, q => q.Include(p => p.SalesItems));
                    var sales = salesDetails.FirstOrDefault();

                    if (sales == null)
                        return Result<AddSalesReturnDetailsResponse>.Failure("SalesDetails not found.");

                    // Validate total quantities
                    var totalOriginalQuantity = sales.SalesItems.Sum(x => x.Quantity);
                    var totalReturnQuantity = request.SalesReturnItems.Sum(x => x.returnQuantity);
                    if (totalReturnQuantity <= 0)
                        return Result<AddSalesReturnDetailsResponse>.Failure("Return quantity must be greater than zero.");



                    var fiscalYear = await _settingServices.GetCurrentFiscalYearBy(companyId, default);
                    var companySettings = await _unitOfWork.BaseRepository<CompanySettings>().FirstOrDefaultAsync(x => x.CompanyId == companyId);
                    if (companySettings == null)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure("Invalid CompanyId. Company does not exist.");
                    }
                    string salesReturnNumber = "";
                    if (companySettings.SalesReturnNumberType == PurchaseSalesReturnNumberType.Manual)
                    {
                        salesReturnNumber = request.salesReturnNumber!;
                    }
                    else
                    {
                        salesReturnNumber = await _billNumberGenerator.GenerateTransactionNumber(companyId, "SALESRETURN", fiscalYear.Data.fyName);
                    }



                    bool salesReturnNumberExists = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                    .AnyAsync(p => p.SalesReturnNumber == salesReturnNumber && p.CompanyId == companyId);



                    if (salesReturnNumberExists)
                    {
                        return Result<AddSalesReturnDetailsResponse>.Failure($"SalesReturn number '{salesReturnNumber}' already exists for this company.");
                    }


                    #region Journal Entries
                    var newJournalId = Guid.NewGuid().ToString();

                    decimal subTotal = request.subTotalAmount ?? 0;
                    decimal vatAmount = sales.VatAmount ?? 0;
                    decimal discount = sales.DiscountAmount ?? 0;
                    decimal taxableAmount = request.taxableAmount ?? subTotal;
                    decimal? amountAfterVat = request.amountAfterVat ?? (subTotal + (vatAmount) - (discount));


                    decimal totalPaymentAmount = amountAfterVat ?? (subTotal + vatAmount - discount);

                    decimal billSundrySalesTotal = 0;
                    decimal billSundryCustomerTotal = 0;

                    var journalDetails = new List<JournalEntryDetails>();
                    var billSundryDetailsList = new List<BillSundryDetails>();
                    var billSundryResponses = new List<CalculationBillResponseDTOs>();

                    // ------------------- REVERSE BILL SUNDRY -------------------
                    if (request.BillSundryIds != null && request.BillSundryIds.Any())
                    {
                        foreach (var bsId in request.BillSundryIds.Where(x => !string.IsNullOrEmpty(x.billSundryIds)))
                        {
                            var calcDto = new CalculationBIllDTOs(bsId.billSundryIds, subTotal, subTotal - discount, amountAfterVat);
                            var bsConfig = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(bsId.billSundryIds);

                            // update default rate if provided
                            if (bsId.rate.HasValue)
                            {
                                bsConfig.DefaultValue = bsId.rate.Value;
                                _unitOfWork.BaseRepository<BillSundry>().Update(bsConfig);
                            }

                            // persist bill sundry detail for return
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
                                return Result<AddSalesReturnDetailsResponse>.Failure("Bill Sundry calculation failed.");

                            var bsResponse = calcResult.Data;

                            // Reverse Bill Sundry amounts
                            if (bsConfig.IsSalesAccountingAffected)
                            {
                                // Reverse Sales Ledger
                                decimal creditAmount = !bsConfig.IsSalesAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                decimal debitAmount = !bsConfig.CustomerAmountAdjusted ? bsResponse.CalculatedAmount : 0;

                                if (creditAmount > 0)
                                {
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        bsConfig.SalesAdjustedLedgerId,
                                        creditAmount,  // Reverse: Debit previous credit
                                        0,
                                        returnedDate,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                }

                                if (debitAmount > 0)
                                {
                                    journalDetails.Add(new JournalEntryDetails(
                                        Guid.NewGuid().ToString(),
                                        newJournalId,
                                        bsConfig.CustomerAdjustedLedgerId,
                                        0,
                                        debitAmount, // Reverse: Credit previous debit
                                        returnedDate,
                                        companyId,
                                        _fiscalContext.CurrentFiscalYearId,
                                        true
                                    ));
                                }
                            }
                        }
                    }
                    else
                    {
                        // If no BillSundryIds provided by user for return, try to reverse original BillSundryDetails proportionally (optional)
                        var originalBillSundries = await _unitOfWork.BaseRepository<BillSundryDetails>()
                            .GetConditionalAsync(x => x.PurchaseSalesAndReturnId == sales.Id);

                        if (originalBillSundries != null && originalBillSundries.Any())
                        {
                            // prorate based on quantity ratio
                            decimal prorate = totalOriginalQuantity > 0 ? Math.Min(1m, totalReturnQuantity / totalOriginalQuantity) : 1m;
                            foreach (var orig in originalBillSundries)
                            {
                                var bsConfig = await _unitOfWork.BaseRepository<BillSundry>().GetByGuIdAsync(orig.BillSundryId);
                                if (bsConfig == null) continue;

                                var calcDto = new CalculationBIllDTOs(orig.BillSundryId, subTotal * prorate, taxableAmount * prorate, amountAfterVat * prorate);
                                var calcResult = await _billSundryServices.CalculateBillSundry(calcDto);
                                if (!calcResult.IsSuccess) continue;
                                var bsResponse = calcResult.Data;

                                var bsDetail = new BillSundryDetails
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    BillSundryId = orig.BillSundryId,
                                    Rate = orig.Rate,
                                    PurchaseSalesAndReturnId = newId
                                };
                                billSundryDetailsList.Add(bsDetail);
                                billSundryResponses.Add(bsResponse);

                                if (bsConfig.IsSalesAccountingAffected)
                                {
                                    // Reverse Sales Ledger
                                    decimal creditAmount = !bsConfig.IsSalesAmountAdjusted ? bsResponse.CalculatedAmount : 0;
                                    decimal debitAmount = !bsConfig.CustomerAmountAdjusted ? bsResponse.CalculatedAmount : 0;

                                    if (creditAmount > 0)
                                    {
                                        journalDetails.Add(new JournalEntryDetails(
                                            Guid.NewGuid().ToString(),
                                            newJournalId,
                                            bsConfig.SalesAdjustedLedgerId,
                                            creditAmount,  // Reverse: Debit previous credit
                                            0,
                                            returnedDate,
                                            companyId,
                                            _fiscalContext.CurrentFiscalYearId,
                                            true
                                        ));
                                    }

                                    if (debitAmount > 0)
                                    {
                                        journalDetails.Add(new JournalEntryDetails(
                                            Guid.NewGuid().ToString(),
                                            newJournalId,
                                            bsConfig.CustomerAdjustedLedgerId,
                                            0,
                                            debitAmount, // Reverse: Credit previous debit
                                            returnedDate,
                                            companyId,
                                            _fiscalContext.CurrentFiscalYearId,
                                            true
                                        ));
                                    }
                                }
                            }

                            if (billSundryDetailsList.Any())
                                await _unitOfWork.BaseRepository<BillSundryDetails>().AddRange(billSundryDetailsList);
                        }

                    }



                        // ------------------- REVERSE SALES LEDGER -------------------
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.SalesLedgerId,
                            subTotal,
                            0,
                            returnedDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));


                    // ------------------- REVERSE VAT -------------------
                    if (vatAmount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.VATLedgerId,
                            vatAmount,
                            0,
                            returnedDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }

                    // ------------------- REVERSE DISCOUNT -------------------
                    if (discount > 0)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            LedgerConstants.DiscountLedgerId,
                            0,
                            discount,
                            returnedDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }




                    // ------------------- CUSTOMER / PAYMENT LEDGER -------------------
                    string ledgerIdToUse = null;
                    var hasPayment = !string.IsNullOrEmpty(request.paymentId);
                    var hasLedger = !string.IsNullOrEmpty(sales.LedgerId);
                    var specialPaymentId = "d1e43e64-9d48-4c85-83f3-3d2d5c9f6c44";
                    var isSpecial = request.paymentId == specialPaymentId;

                    // Helper method to add journal
                    void AddLedger(string ledgerId, decimal debit, decimal credit)
                    {
                        journalDetails.Add(new JournalEntryDetails(
                            Guid.NewGuid().ToString(),
                            newJournalId,
                            ledgerId,
                            debit,
                            credit,
                            returnedDate,
                            companyId,
                            _fiscalContext.CurrentFiscalYearId,
                            true
                        ));
                    }

                    // Determine ledger to use
                    if (isSpecial && hasLedger)
                    {
                        ledgerIdToUse = sales.LedgerId;
                    }
                    else if (hasPayment)
                    {
                        var paymentMethod = await _unitOfWork.BaseRepository<PaymentMethod>().GetByGuIdAsync(request.paymentId);
                        var subledgerGroup = await _unitOfWork.BaseRepository<SubLedgerGroup>()
                            .GetByGuIdAsync(paymentMethod.SubLedgerGroupsId)
                            ?? throw new Exception("Ledger group not found");

                        var ledger = await _unitOfWork.BaseRepository<Ledger>()
                            .FirstOrDefaultAsync(x =>x.IsActive && x.SubLedgerGroupId == subledgerGroup.Id);

                        ledgerIdToUse = subledgerGroup.Id switch
                        {
                            SubLedgerGroupConstants.CashInHands => ledger?.Id ?? LedgerConstants.CashLedgerId,
                            SubLedgerGroupConstants.BankAccounts => ledger?.Id ?? LedgerConstants.BankLedgerId,
                            _ => ledger?.Id ?? sales.LedgerId
                        };
                    }
                    else if (hasLedger)
                    {
                        ledgerIdToUse = sales.LedgerId;
                    }

                    // Add Payment / Customer Ledger
                    decimal totalCreditSoFar = journalDetails.Sum(x => x.CreditAmount);
                    decimal totalDebitSoFar = journalDetails.Sum(x => x.DebitAmount);
                    decimal customerCredit = totalDebitSoFar - totalCreditSoFar;

                    if (!string.IsNullOrEmpty(ledgerIdToUse))
                    {
                        AddLedger(ledgerIdToUse, 0, customerCredit); // Reverse: Customer receives refund
                    }

                    // ------------------- COGS / STOCK ADJUSTMENT -------------------
                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        LedgerConstants.CostOfGoodSoldLedgerId,
                        subTotal,
                        0,
                        returnedDate,
                        companyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));

                    journalDetails.Add(new JournalEntryDetails(
                        Guid.NewGuid().ToString(),
                        newJournalId,
                        LedgerConstants.StockLedgerId,
                        0,
                        subTotal,
                        returnedDate,
                        companyId,
                        _fiscalContext.CurrentFiscalYearId,
                        true
                    ));

                    // ------------------- BALANCE CHECK -------------------
                    decimal totalDebit = journalDetails.Sum(x => x.DebitAmount);
                    decimal totalCredit = journalDetails.Sum(x => x.CreditAmount);

                    if (totalDebit != totalCredit)
                        throw new InvalidOperationException($"Sales Return Journal unbalanced! Debit={totalDebit}, Credit={totalCredit}");








                    #endregion



                    if (salesDetails == null)
                        throw new Exception("SalesDetails not found.");

                    var salesReturnDetailsData = new SalesReturnDetails(
                            newId,
                       request.salesDetailsId,
                       nepaliDate,
                       request.totalReturnAmount,
                       request.taxAdjustment,
                       request.discount,
                       request.netReturnAmount,
                       request.reason,
                       companyId,
                       sales.StockCenterId,
                       userId,
                       DateTime.UtcNow,
                       "",
                       default,
                       sales.LedgerId,
                       newJournalId,
                       salesReturnNumber,
                      request.SalesReturnItems?
                        .Where(d => !string.IsNullOrWhiteSpace(d.salesItemsId))
                        .Select(d => new SalesReturnItems(
                            Guid.NewGuid().ToString(),
                            newId,
                            d.salesItemsId,
                            d.returnQuantity,
                            d.returnUnitPrice,
                            d.returnTotalAmount,
                            userId,
                            DateTime.Now.ToString(),
                            "",
                            ""
                        )).ToList() ?? new List<SalesReturnItems>()

                    );
                    await _unitOfWork.BaseRepository<SalesReturnDetails>().AddAsync(salesReturnDetailsData);

                    var quantityReturned = salesReturnDetailsData.SalesReturnItems.Sum(x => x.ReturnQuantity);

                    var quantityToBeReturnedItemList = await _unitOfWork.BaseRepository<SalesDetails>().GetConditionalAsync
                        (x => x.Id == request.salesDetailsId,
                        query => query.Include(x => x.SalesItems));

                    var quantityToBeReturned = quantityToBeReturnedItemList
                        .SelectMany(x => x.SalesItems)
                        .Sum(item => item.Quantity);

                    var salesDetailsToBeUpdated = await _unitOfWork.BaseRepository<SalesDetails>().GetByGuIdAsync(request.salesDetailsId);

                    if (quantityReturned == quantityToBeReturned && quantityReturned > 0)
                    {
                        salesDetailsToBeUpdated.Status = SalesStatus.Returned;
                    }
                    else if (quantityReturned < quantityToBeReturned && quantityReturned > 0)
                    {
                        salesDetailsToBeUpdated.Status = SalesStatus.PartiallyReturned;
                    }
                    _unitOfWork.BaseRepository<SalesDetails>().Update(salesDetailsToBeUpdated);


         


                    sales.GrandTotalAmount += request.netReturnAmount;



                    if (sales.GrandTotalAmount == 0)
                    {
                        _unitOfWork.BaseRepository<SalesDetails>().Delete(sales);
                    }
                    sales.VatAmount =
                    sales.VatAmount + request.taxAdjustment;

                    sales.DiscountAmount =
                    sales.DiscountAmount + request.discount;



                    if (request.SalesReturnItems != null && request.SalesReturnItems.Any())
                    {
                        foreach (var detail in request.SalesReturnItems)
                        {
                            var existingSales = await _unitOfWork.BaseRepository<SalesItems>().GetByGuIdAsync(detail.salesItemsId);


                            if (existingSales != null)
                            {
                                if (detail.returnQuantity <= 0 || detail.returnQuantity > existingSales.Quantity)
                                    throw new InvalidOperationException("Invalid return quantity.");


                                existingSales.Quantity -= detail.returnQuantity;
                                existingSales.Price = detail.returnQuantity > 0
                                ? detail.returnTotalAmount / detail.returnQuantity
                                : 0;
                                existingSales.Amount -= detail.returnTotalAmount;
                                if (existingSales.Quantity == 0)
                                {
                                    existingSales.IsActive = false;
                                    _unitOfWork.BaseRepository<SalesItems>().Update(existingSales);
                                }
                                else
                                    _unitOfWork.BaseRepository<SalesItems>().Update(existingSales);
                            }

                            #region ItemInstances
                            var itemInstance = await _unitOfWork.BaseRepository<ItemInstance>()
                                .FirstOrDefaultAsync(x => x.SalesItemsId == detail.salesItemsId && x.ItemsId == detail.itemsId);

                            if (itemInstance != null)
                            {
                                _unitOfWork.BaseRepository<ItemInstance>()
                                     .Delete(itemInstance);
                            }

                            #endregion

                            #region Inventory

                            var salesItemIds = request.SalesReturnItems
                                .Select(x => x.salesItemsId)
                                .Distinct()
                                .ToList();

                            var ItemIds = request.SalesReturnItems
                                .Select(x => x.itemsId)
                                .Distinct()
                                .ToList();

                            var inventories = await _unitOfWork.BaseRepository<Inventories>()
                                .FirstOrDefaultAsync(x => salesItemIds.Contains(x.SalesItemsId) && ItemIds.Contains(x.ItemId));




                            if (inventories != null)
                            {
                                inventories.QuantityOut += detail.returnQuantity;
                                inventories.AmountOut += detail.returnTotalAmount;

                                if (inventories.QuantityIn == 0)
                                    _unitOfWork.BaseRepository<Inventories>().Delete(inventories);
                                else
                                    _unitOfWork.BaseRepository<Inventories>().Update(inventories);
                            }
                            else
                            {
                                //Incomplete for salesItemsId
                                var inventoryLogs = await _unitOfWork.BaseRepository<InventoriesLogs>()
                                    .FirstOrDefaultAsync(x => salesItemIds.Contains(x.PurchaseItemsId) && ItemIds.Contains(x.ItemId));

                                if (inventoryLogs != null)
                                {
                                    inventoryLogs.QuantityIn += detail.returnQuantity;
                                    inventoryLogs.AmountIn += detail.returnTotalAmount;

                                    if (inventoryLogs.QuantityIn == 0)
                                        _unitOfWork.BaseRepository<InventoriesLogs>().Delete(inventoryLogs);
                                    else
                                        _unitOfWork.BaseRepository<InventoriesLogs>().Update(inventoryLogs);
                                }


                            }

                            #endregion


                        }
                    }
               
                  


                    #region Payment Added
                    PaymentsDetails payment;

                    var paymentMethods = await _unitOfWork.BaseRepository<PaymentMethod>()
                       .GetByGuIdAsync(request.paymentId);

                    if (paymentMethods.SubLedgerGroupsId == SubLedgerGroupConstants.BankAccounts)
                    {
                        payment = new ChequePayment(
                            id: Guid.NewGuid().ToString(),
                            transactionType: TransactionType.SalesReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: totalPaymentAmount,
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
                            transactionType: TransactionType.SalesReturn,
                            transactionDate: DateTime.Now,
                            totalAmount: totalPaymentAmount,
                            transactionDetailsId: newId,
                            paymentMethodId: request.paymentId,
                            companyId: companyId
                        );
                    }

                    await _unitOfWork.BaseRepository<PaymentsDetails>().AddAsync(payment);

                    #endregion



                    await _unitOfWork.SaveChangesAsync();




                    scope.Complete();

                    var resultDTOs = _mapper.Map<AddSalesReturnDetailsResponse>(salesReturnDetailsData);
                    return Result<AddSalesReturnDetailsResponse>.Success(resultDTOs);

                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw new Exception("An error occurred while adding Sales Return Details ", ex);

                }
            }

        }

        public async Task<Result<bool>> Delete(string id, CancellationToken cancellationToken)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);
                if (salesReturnDetails is null)
                {
                    return Result<bool>.Failure("NotFound", "sales ReturnDetails  Cannot be Found");
                }


                var originalJournals = await _unitOfWork.BaseRepository<JournalEntry>()
                    .GetConditionalAsync(x => x.Id == salesReturnDetails.JournalEntriesId,
                    query => query.Include(j => j.JournalEntryDetails)
                    );
                var originalJournal = originalJournals.FirstOrDefault();
                _unitOfWork.BaseRepository<JournalEntry>().Delete(originalJournal);
                _unitOfWork.BaseRepository<SalesReturnDetails>().Delete(salesReturnDetails);
                await _unitOfWork.SaveChangesAsync();


                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting sales Return Details having {id}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>> GetAllSalesReturnDetails(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>()
              .GetConditionalAsync(
                null,
              query => query.Include(rm => rm.SalesReturnItems)
              .ThenInclude(i => i.SalesItems)
              ) ?? new List<SalesReturnDetails>();



                var salesReturnDetailsResponses = salesReturnDetails.Select(salesReturn =>
                {
                    decimal subTotalAmount = salesReturn.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - salesReturn.Discount;

                    return new GetAllSalesReturnDetailsByQueryResponse(
                        salesReturn.Id,
                        salesReturn.SalesDetailsId,
                        salesReturn.ReturnDate,
                        salesReturn.TotalReturnAmount,
                        salesReturn.TaxAdjustment,
                        salesReturn.Discount,
                        salesReturn.NetReturnAmount,
                        salesReturn.Reason,
                        salesReturn.CreatedBy,
                        salesReturn.CreatedAt,
                        salesReturn.ModifiedBy,
                        salesReturn.ModifiedAt,
                        salesReturn.SalesReturnItems?.Count() ?? 0,
                        salesReturn.LedgerId,
                        salesReturn.CompanyId,
                        salesReturn.StockCenterId,
                          taxableAmount,
      subTotalAmount,
                        salesReturn.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                            items.Id,
                            items.SalesReturnDetailsId,
                            items.SalesItemsId,
                            items.SalesItems.ItemId,
                            items.ReturnQuantity,
                            items.ReturnUnitPrice,
                            items.ReturnTotalPrice
                        )).ToList() ?? new List<SalesReturnItemsDto>()
                    );
                }).ToList();



                var institutionId = _tokenService.InstitutionId() ?? string.Empty;

                var isSuperAdmin = _tokenService.GetRole() == Role.SuperAdmin;


                var filterSalesReturnDetails = isSuperAdmin ? salesReturnDetailsResponses : salesReturnDetailsResponses.Where(x => x.companyId == _tokenService.CompanyId().FirstOrDefault());

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                   .GetConditionalFilterType(
                       x => x.InstitutionId == institutionId,
                       query => query.Select(c => c.Id)
                   );


                if (!string.IsNullOrEmpty(institutionId) && string.IsNullOrEmpty(_tokenService.CompanyId().FirstOrDefault()))
                {
                    var filteredSalesReturnEntries = await _unitOfWork.BaseRepository<SalesReturnDetails>()
                        .GetConditionalAsync(x => companyIds.Contains(x.CompanyId),
                        query => query.Include(j => j.SalesReturnItems)
                        );

                    filterSalesReturnDetails = filteredSalesReturnEntries
                        .Select(salesReturn =>
                        {
                            decimal subTotalAmount = salesReturn.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                            decimal? taxableAmount = subTotalAmount - salesReturn.Discount;
                            return new GetAllSalesReturnDetailsByQueryResponse(
                                     salesReturn.Id,
                             salesReturn.SalesDetailsId,
                             salesReturn.ReturnDate,
                             salesReturn.TotalReturnAmount,
                             salesReturn.TaxAdjustment,
                             salesReturn.Discount,
                             salesReturn.NetReturnAmount,
                             salesReturn.Reason,
                             salesReturn.CreatedBy,
                             salesReturn.CreatedAt,
                             salesReturn.ModifiedBy,
                             salesReturn.ModifiedAt,
                             salesReturn.SalesReturnItems?.Count() ?? 0,
                             salesReturn.LedgerId,
                             salesReturn.CompanyId,
                             salesReturn.StockCenterId,
                                   taxableAmount,
      subTotalAmount,
                             salesReturn.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                                 items.Id,
                                 items.SalesReturnDetailsId,
                                 items.SalesItemsId,
                                 items.SalesItems.Item.Id,
                                 items.ReturnQuantity,
                                 items.ReturnUnitPrice,
                                 items.ReturnTotalPrice
                             )).ToList() ?? new List<SalesReturnItemsDto>()
                         );
                        }
                        ).ToList();


                }

                var totalItems = filterSalesReturnDetails.Count();

                var paginatedSalesReturnDetailsEntries = paginationRequest != null && paginationRequest.IsPagination
                    ? filterSalesReturnDetails
                        .Skip((paginationRequest.pageIndex - 1) * paginationRequest.pageSize)
                        .Take(paginationRequest.pageSize)
                        .ToList()
                    : filterSalesReturnDetails.ToList();

                var pagedResult = new PagedResult<GetAllSalesReturnDetailsByQueryResponse>
                {
                    Items = paginatedSalesReturnDetailsEntries,
                    TotalItems = totalItems,
                    PageIndex = paginationRequest?.pageIndex ?? 1,
                    pageSize = paginationRequest?.pageSize ?? totalItems
                };

                return Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>.Success(pagedResult);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Sales Return Details", ex);
            }
        }

        public async Task<Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>> SalesReturnDetailsFilters(PaginationRequest paginationRequest, FilterSalesReturnDetailsDTOs filterSalesReturnDetailsDTOs)
        {

            try
            {
                // Get scoped user data
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) = await _getUserScopedData.GetUserScopedData<SalesReturnDetails>();

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                    );

                // Parse date range using Nepali date conversion
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterSalesReturnDetailsDTOs.startDate?.Trim()))
                {
                    startDateUtc = await _dateConverterHelper.ConvertToEnglish(filterSalesReturnDetailsDTOs.startDate.Trim());
                    if (DateTime.TryParse(filterSalesReturnDetailsDTOs.startDate, out var tempStart) && tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(filterSalesReturnDetailsDTOs.endDate?.Trim()))
                {
                    endDateUtc = await _dateConverterHelper.ConvertToEnglish(filterSalesReturnDetailsDTOs.endDate.Trim());
                    if (DateTime.TryParse(filterSalesReturnDetailsDTOs.endDate, out var tempEnd) && tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // If both dates are invalid, default to today's full day
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

               
                // Filter by user
                var userId = _tokenService.GetUserId();

                var salesReturnResult = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetConditionalAsync(
                    predicate: x =>
                        x.CreatedBy == userId &&
                        (string.IsNullOrEmpty(filterSalesReturnDetailsDTOs.ledgerId) || x.LedgerId.ToLower().Contains(filterSalesReturnDetailsDTOs.ledgerId.ToLower())) &&
                        (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                        (endDateUtc == null || x.CreatedAt < endDateUtc),
                    queryModifier: q => q.Include(r => r.SalesReturnItems)
                    .ThenInclude(x=>x.SalesItems)
                    .ThenInclude(x=>x.Item)
                );

                // Mapping
                var responseList = salesReturnResult.Select(sr =>
                {
                    decimal subTotalAmount = sr.SalesReturnItems?.Sum(i => i.ReturnTotalPrice) ?? 0m;

                    decimal? taxableAmount = subTotalAmount - sr.Discount;

                    return new GetSalesReturnDetailsFilterQueryResponse(
                        sr.Id,
                        sr.SalesDetailsId,
                        sr.ReturnDate,
                        sr.TotalReturnAmount,
                        sr.TaxAdjustment,
                        sr.Discount,
                        sr.NetReturnAmount,
                        sr.Reason,
                        sr.CreatedBy,
                        sr.CreatedAt,
                        sr.ModifiedBy,
                        sr.ModifiedAt,
                        sr.LedgerId,
                        sr.CompanyId,
                        sr.StockCenterId,
                              taxableAmount,
      subTotalAmount,
                        sr.SalesReturnItems?.Select(items => new SalesReturnItemsDto(
                            items.Id,
                            items.SalesReturnDetailsId,
                            items.SalesItemsId,
                            items.SalesItems?.Item?.Id ?? string.Empty,
                            items.ReturnQuantity,
                            items.ReturnUnitPrice,
                            items.ReturnTotalPrice
                        )).ToList() ?? new List<SalesReturnItemsDto>()
                    );
                }).ToList();

                PagedResult<GetSalesReturnDetailsFilterQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetSalesReturnDetailsFilterQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetSalesReturnDetailsFilterQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching sales return details: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>> GetAllSalesReturnItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnItems = await _unitOfWork.BaseRepository<SalesReturnItems>().GetAllAsyncWithPagination();
                var salesReturnItemsPagedResult = await salesReturnItems.AsNoTracking().ToPagedResultAsync(paginationRequest.pageIndex, paginationRequest.pageSize, paginationRequest.IsPagination);

                var allSalesReturnItemsDisplay = _mapper.Map<PagedResult<GetAllSalesReturnItemsByQueryResponse>>(salesReturnItemsPagedResult.Data);

                return Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>.Success(allSalesReturnItemsDisplay);

            }
            catch (Exception ex)

            {
                throw new Exception("An error occurred while fetching all Sales Return Items", ex);
            }
        }

        public async Task<Result<GetSalesReturnDetailsByIdQueryResponse>> GetSalesReturnDetailsById(string id, CancellationToken cancellationToken = default)
        {
            try
            {

                var salesReturnDetails = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);

                var salesPayment = await _unitOfWork.BaseRepository<ChequePayment>()
                    .GetConditionalAsync(x => x.TransactionDetailsId == id && x.TransactionType == TransactionType.SalesReturn);

                var salesPaymentDetails = salesPayment.FirstOrDefault();

                if (salesReturnDetails == null)
                {
                    return Result<GetSalesReturnDetailsByIdQueryResponse>.Failure("Sales return details not found.");
                }

                var salesReturnDetailsResponse = _mapper.Map<GetSalesReturnDetailsByIdQueryResponse>(salesReturnDetails);

                var updatedResponse = salesPaymentDetails is not null
                       ? salesReturnDetailsResponse with
                       {
                           chequeNumber = salesPaymentDetails.ChequeNumber,
                           bankName = salesPaymentDetails.BankName,
                           accountName = salesPaymentDetails.AccountName
                       }
                       : salesReturnDetailsResponse;

                return Result<GetSalesReturnDetailsByIdQueryResponse>.Success(updatedResponse);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching SalesReturn Details by using Id", ex);
            }
        }

        public async Task<Result<UpdateSalesReturnDetailsResponse>> Update(UpdateSalesReturnDetailsCommand request, string id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (id == null)
                    {
                        return Result<UpdateSalesReturnDetailsResponse>.Failure("NotFound", "Please provide valid sales return Details id");
                    }

                    var salesReturnDetailsTobeUpdated = await _unitOfWork.BaseRepository<SalesReturnDetails>().GetByGuIdAsync(id);
                    if (salesReturnDetailsTobeUpdated is null)
                    {
                        return Result<UpdateSalesReturnDetailsResponse>.Failure("NotFound", "SalesReturn details  are not Found");
                    }

                    _mapper.Map(request, salesReturnDetailsTobeUpdated);
                    await _unitOfWork.SaveChangesAsync();
                    scope.Complete();

                    var resultResponse = new UpdateSalesReturnDetailsResponse
                        (
                            id,
                            salesReturnDetailsTobeUpdated.SalesDetailsId,
                            salesReturnDetailsTobeUpdated.ReturnDate,
                            salesReturnDetailsTobeUpdated.TotalReturnAmount,
                            salesReturnDetailsTobeUpdated.TaxAdjustment,
                            salesReturnDetailsTobeUpdated.NetReturnAmount,
                            salesReturnDetailsTobeUpdated.Reason,
                            salesReturnDetailsTobeUpdated.CreatedBy,
                            salesReturnDetailsTobeUpdated.CreatedAt,
                            salesReturnDetailsTobeUpdated.ModifiedBy,
                            salesReturnDetailsTobeUpdated.ModifiedAt

                        );

                    return Result<UpdateSalesReturnDetailsResponse>.Success(resultResponse);

                }
                catch (Exception ex)
                {
                    throw new Exception("an error occurred while updating sales return Details ");
                }
            }
        }
    }
}
