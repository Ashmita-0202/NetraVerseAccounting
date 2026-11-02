using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NV.Payment.Domain.Entities;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.FilterLedger;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.DayBook.CashBook;
using TN.Reports.Application.DayBook.FilterPurchaseDayBook;
using TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook;
using TN.Reports.Application.GodownwiseInventoryReport;
using TN.Reports.Application.ItemRateHistory;
using TN.Reports.Application.ItemwiseProfitReport;
using TN.Reports.Application.ItemwisePurchaseByExpireDate;
using TN.Reports.Application.ItemwisePurchaseReport;
using TN.Reports.Application.PurchaseReport;
using TN.Reports.Application.PurchaseReturnReport;
using TN.Reports.Application.PurchaseSummaryReport;
using TN.Reports.Application.ServiceInterface;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Payments;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using static TN.Purchase.Domain.Entities.PurchaseDetails;
using static TN.Sales.Domain.Entities.SalesDetails;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;


namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class PurchaseReportService : IPurchaseReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IGetUserScopedData _getUserScopedData;
        private readonly IDateConvertHelper _dateConvertHelper;

        public PurchaseReportService(IDateConvertHelper dateConvertHelper, IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService, IGetUserScopedData getUserScopedData)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
            _getUserScopedData = getUserScopedData;
            _dateConvertHelper = dateConvertHelper;

        }

        public async Task<PagedResult<GetPurchaseReportQueryResponse>> GetAllPurchaseReport(PaginationRequest paginationRequest, PurchaseReportDtos purchaseReportDtos)
        {
            try
            {
                var (purchaseQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();


                IQueryable<PurchaseDetails> filteredPurchase = purchaseQuery;

                if (!string.IsNullOrEmpty(purchaseReportDtos.companyId))
                {
                    filteredPurchase = filteredPurchase.Where(x => x.CompanyId == purchaseReportDtos.companyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filteredPurchase = filteredPurchase.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filteredPurchase = filteredPurchase.Where(x => companyIds.Contains(x.CompanyId));
                }

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(purchaseReportDtos.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReportDtos.startDate.Trim());

                    if (DateTime.TryParse(purchaseReportDtos.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }


                if (!string.IsNullOrWhiteSpace(purchaseReportDtos.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReportDtos.endDate.Trim());

                    if (DateTime.TryParse(purchaseReportDtos.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }


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


                filteredPurchase = filteredPurchase.Where(s => s.CreatedAt >= startDateUtc && s.CreatedAt < endDateUtc);

                filteredPurchase = filteredPurchase.Where(s =>
                     (string.IsNullOrEmpty(purchaseReportDtos.stockCenterId) || s.StockCenterId == purchaseReportDtos.stockCenterId) &&
                     (string.IsNullOrEmpty(purchaseReportDtos.itemGroupId) || s.PurchaseItems.Any(i => i.Item.ItemGroupId == purchaseReportDtos.itemGroupId)) &&
                     (string.IsNullOrEmpty(purchaseReportDtos.billNumber) || s.BillNumber == purchaseReportDtos.billNumber) &&
                     (string.IsNullOrEmpty(purchaseReportDtos.ledgerId) || s.LedgerId == purchaseReportDtos.ledgerId) &&
                     (string.IsNullOrEmpty(purchaseReportDtos.ItemId) || s.PurchaseItems.Any(i => i.ItemId == purchaseReportDtos.ItemId)) &&
                     (purchaseReportDtos.SerialNumbers == null || !purchaseReportDtos.SerialNumbers.Any() ||
                         s.PurchaseItems.Any(i => i.ItemInstances.Any(ii => !string.IsNullOrEmpty(ii.SerialNumber) &&
                                                                             purchaseReportDtos.SerialNumbers.Contains(ii.SerialNumber))))
                );

                //int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                //int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                //var totalItems = await filteredPurchase.CountAsync();

                // --- Fetch and project ---
                var pagedSales = await filteredPurchase

                    .Include(s => s.PurchaseItems)
                        .ThenInclude(si => si.ItemInstances)
                    .OrderByDescending(s => s.CreatedAt)
                    //.Skip((pageIndex - 1) * pageSize)
                    //.Take(pageSize)
                    .SelectMany(s => s.PurchaseItems.Select(si => new GetPurchaseReportQueryResponse(
                        si.ItemId,
                        si.Item.ItemGroupId ?? "",
                        si.ItemInstances
                            .Where(ii => !string.IsNullOrEmpty(ii.SerialNumber) && ii.PurchaseItemsId == si.Id)
                            .Select(ii => ii.SerialNumber)
                            .ToList(),
                        s.Date ?? string.Empty,
                        s.BillNumber ?? string.Empty,
                        s.LedgerId ?? string.Empty,
                        si.Quantity,
                        si.Price,
                        si.Quantity * si.Price,
                        s.DiscountAmount,
                        s.VatAmount,
                        s.GrandTotalAmount,
                        s.StockCenterId ?? string.Empty
                    )))
                    .ToListAsync();

                PagedResult<GetPurchaseReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = pagedSales.Count();

                    var pagedItems = pagedSales
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetPurchaseReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetPurchaseReportQueryResponse>
                    {
                        Items = pagedSales.ToList(),
                        TotalItems = pagedSales.Count(),
                        PageIndex = 1,
                        pageSize = pagedSales.Count()
                    };
                }


                return finalResponseList;

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the report.", ex);
            }
        }

        public async Task<Result<PagedResult<CashDayBookQueryResponse>>> GetCashDayBook(PaginationRequest paginationRequest, CashDayBookDto cashDayBookDto, CancellationToken cancellationToken = default)
        {
            try
            {

                // 🔹 Get user-scoped data
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PaymentsDetails>();

               

                //// 🔹 Setup date range
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(cashDayBookDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(cashDayBookDto.startDate.Trim());

                    if (DateTime.TryParse(cashDayBookDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(cashDayBookDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(cashDayBookDto.endDate.Trim());

                    if (DateTime.TryParse(cashDayBookDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

                // 🔹 Handle missing or invalid dates
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

                var payments = await _unitOfWork.BaseRepository<PaymentsDetails>()
                    .GetConditionalAsync(

                        p => p.Id == p.Id &&
                             (startDateUtc == null || p.TransactionDate >= startDateUtc) &&
                             (endDateUtc == null || p.TransactionDate < endDateUtc)
                             && (string.IsNullOrEmpty(companyId) || p.CompanyId == companyId),
                         
                         q => q
                        .Include(p => p.PaymentMethod)
                            .ThenInclude(td => td.TransactionDetails)
                            .ThenInclude(d => d.TransactionsItems)
                        .AsNoTracking().Include(p => p.PaymentMethod)
                .ThenInclude(pm => pm.Payments)

                    );

                // 🔹 Calculate running balance and build response
                decimal runningBalance = 0;
                var responseList = new List<CashDayBookQueryResponse>();

                foreach (var p in payments.OrderBy(p => p.TransactionDate))
                {
                    decimal debit = 0, credit = 0;

                    string ledgerId = p.PaymentMethod.TransactionDetails
                        ?.SelectMany(td => td.TransactionsItems)
                        ?.FirstOrDefault()?.LedgerId ?? "";

                    if (p.TransactionType == TransactionType.Income ||
                        p.TransactionType == TransactionType.Receipts ||
                        p.TransactionType == TransactionType.Sales)
                        debit = p.TotalAmount;
                    else if (p.TransactionType == TransactionType.Expense ||
                             p.TransactionType == TransactionType.Payment ||
                             p.TransactionType == TransactionType.Purchase)
                        credit = p.TotalAmount;

                    runningBalance += debit - credit;

                    // 🔹 Fetch reference number only for Purchase and Sales
                    string referenceNumber = "";

                    switch (p.TransactionType)
                    {
                        case TransactionType.Purchase:
                            var purchase = await _unitOfWork.BaseRepository<PurchaseDetails>()
                                .FirstOrDefaultAsync(x => x.PaymentId == p.Id);
                            referenceNumber = purchase?.ReferenceNumber ?? "";
                            break;

                        case TransactionType.Sales:
                            var sale = await _unitOfWork.BaseRepository<SalesDetails>()
                                .FirstOrDefaultAsync(x => x.PaymentId == p.Id);
                            referenceNumber = sale?.BillNumber ?? "";
                            break;

                        case TransactionType.Receipts:
                        case TransactionType.Payment:
                        case TransactionType.Income:
                        case TransactionType.Expense:
                           
                            referenceNumber = p.PaymentMethod.TransactionDetails
                                ?.FirstOrDefault()?.TransactionNumber ?? "";
                            break;

                        default:
                            referenceNumber = "";
                            break;
                    }

                    responseList.Add(new CashDayBookQueryResponse(
                        p.TransactionDate,
                        referenceNumber,
                        p.TransactionType,
                        ledgerId,
                        p.PaymentMethodId,
                        debit,
                        credit,
                        runningBalance
                    ));
                }

                PagedResult<CashDayBookQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<CashDayBookQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<CashDayBookQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }

                return Result<PagedResult<CashDayBookQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Cash Day Book: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<FilterPurchaseDayBookQueryResponse>>> GetFilterPurchaseDayBook(PaginationRequest paginationRequest, FilterPurchaseDayBookDto filterPurchaseDayBookDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (purchaseDayBook, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();
                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();

                var filterDayBook = isSuperAdmin
                    ? purchaseDayBook
                    : purchaseDayBook.Where(x => x.CompanyId == currentCompanyId);

                var companyIds = await _unitOfWork.BaseRepository<Company>()
                    .GetConditionalFilterType(
                        x => x.InstitutionId == institutionId,
                        query => query.Select(c => c.Id)
                );

                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(filterPurchaseDayBookDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(filterPurchaseDayBookDto.startDate.Trim());

                    if (DateTime.TryParse(filterPurchaseDayBookDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }


                if (!string.IsNullOrWhiteSpace(filterPurchaseDayBookDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(filterPurchaseDayBookDto.endDate.Trim());

                    if (DateTime.TryParse(filterPurchaseDayBookDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }


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

                var userId = _tokenService.GetUserId;


                var purchaseDayBookResult = await _unitOfWork.BaseRepository<PurchaseDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId() &&

                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc),
                        queryModifier: q => q.AsNoTracking()
                    );


                var responseList = purchaseDayBookResult.Select(sd => new FilterPurchaseDayBookQueryResponse(
                         sd.Date ?? sd.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                         sd.BillNumber ?? "",
                         sd.ReferenceNumber,
                         sd.LedgerId ?? "",
                         netAmount: (sd.GrandTotalAmount - (sd.VatAmount ?? 0)),
                         sd.VatAmount,
                         sd.GrandTotalAmount

                    )).ToList();


                PagedResult<FilterPurchaseDayBookQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<FilterPurchaseDayBookQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<FilterPurchaseDayBookQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }


                return Result<PagedResult<FilterPurchaseDayBookQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Purchase day book: {ex.Message}", ex);
            }
        }

        public Task<Result<PagedResult<GetGodownwiseInventoryQueryResponse>>> GetGodownwiseInventory(PaginationRequest paginationRequest, GodownwiseInventoryDtos godownwiseInventoryDtos, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<PagedResult<GetItemRateHistoryQueryResponse>>> GetItemRateHistory(PaginationRequest paginationRequest, ItemRateHistoryDtos itemRateHistoryDtos, CancellationToken cancellationToken = default)
        {
           try
    {
        var (inventoryQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
            await _getUserScopedData.GetUserScopedData<Inventories>();

        IQueryable<Inventories> filteredInventories = inventoryQuery;

        // Company scoping
        if (!string.IsNullOrEmpty(itemRateHistoryDtos.stockCenterId))
        {
            filteredInventories = filteredInventories
                .Where(x => x.companyId == itemRateHistoryDtos.stockCenterId);
        }
        else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
        {
            filteredInventories = filteredInventories
                .Where(x => x.companyId == currentCompanyId);
        }
        else if (!string.IsNullOrEmpty(institutionId))
        {
            var companyIds = await _unitOfWork.BaseRepository<Company>()
                .GetConditionalFilterType(
                    x => x.InstitutionId == institutionId,
                    query => query.Select(c => c.Id)
                );

            filteredInventories = filteredInventories.Where(x => companyIds.Contains(x.companyId));
        }

        // Date filters
        DateTime? startDateUtc = null;
        DateTime? endDateUtc = null;

        if (!string.IsNullOrWhiteSpace(itemRateHistoryDtos.startDate?.Trim()))
        {
            startDateUtc = await _dateConvertHelper.ConvertToEnglish(itemRateHistoryDtos.startDate.Trim());

            if (DateTime.TryParse(itemRateHistoryDtos.startDate, out var tempStart) &&
                tempStart.TimeOfDay == TimeSpan.Zero)
            {
                endDateUtc = startDateUtc?.AddDays(1);
            }
        }

        if (!string.IsNullOrWhiteSpace(itemRateHistoryDtos.endDate?.Trim()))
        {
            endDateUtc = await _dateConvertHelper.ConvertToEnglish(itemRateHistoryDtos.endDate.Trim());

            if (DateTime.TryParse(itemRateHistoryDtos.endDate, out var tempEnd) &&
                tempEnd.TimeOfDay == TimeSpan.Zero)
            {
                endDateUtc = endDateUtc?.AddDays(1);
            }
        }

        // Only apply date filter if user provided at least one valid date
        if (startDateUtc.HasValue && endDateUtc.HasValue)
        {
            filteredInventories = filteredInventories
                .Where(i => i.CreatedAt >= startDateUtc && i.CreatedAt < endDateUtc);
        }
        else if (startDateUtc.HasValue && !endDateUtc.HasValue)
        {
            // filter only from start date onwards
            filteredInventories = filteredInventories
                .Where(i => i.CreatedAt >= startDateUtc);
        }
        else if (!startDateUtc.HasValue && endDateUtc.HasValue)
        {
            // filter only up to end date
            filteredInventories = filteredInventories
                .Where(i => i.CreatedAt < endDateUtc);
        }
        // 👉 if no dates provided, no date filter applied

        // Item, group, ledger, type filters
        filteredInventories = filteredInventories.Where(i =>
            (string.IsNullOrEmpty(itemRateHistoryDtos.itemId) || i.ItemId == itemRateHistoryDtos.itemId) &&
            (string.IsNullOrEmpty(itemRateHistoryDtos.itemGroupId) || i.Items.ItemGroupId == itemRateHistoryDtos.itemGroupId) &&
            (string.IsNullOrEmpty(itemRateHistoryDtos.ledgerId) || i.LedgerId == itemRateHistoryDtos.ledgerId) &&
            (!itemRateHistoryDtos.type.HasValue || i.Type == itemRateHistoryDtos.type.Value)
        );

        // Load filtered inventories with items
        var inventories = await filteredInventories
            .Include(i => i.Items)
            .ToListAsync(cancellationToken);

        // Load purchase and sales items + their details
        var purchaseDetails = await _unitOfWork.BaseRepository<PurchaseDetails>()
            .GetConditionalAsync(queryModifier: q => q.Include(p => p.PurchaseItems));

        var salesDetails = await _unitOfWork.BaseRepository<SalesDetails>()
            .GetConditionalAsync(queryModifier: q => q.Include(s => s.SalesItems));

        var allPurchaseItems = purchaseDetails.SelectMany(pd => pd.PurchaseItems).ToList();
        var allSalesItems = salesDetails.SelectMany(sd => sd.SalesItems).ToList();

        // Transform data
        var inventoryRecords = inventories.Select(inv =>
        {
            string? billNumber = null;
            decimal quantity = 0;
            decimal amount = 0;
            decimal price = 0;

            if (inv.Type == Inventories.InventoriesType.Purchase && !string.IsNullOrEmpty(inv.PurchaseItemsId))
            {
                var purchaseItem = allPurchaseItems.FirstOrDefault(pi => pi.Id == inv.PurchaseItemsId);
                if (purchaseItem != null)
                {
                    var pd = purchaseDetails.FirstOrDefault(p => p.Id == purchaseItem.PurchaseDetailsId);
                    billNumber = pd?.BillNumber;
                }

                quantity = inv.QuantityIn;
                amount = inv.AmountIn;
                price = quantity != 0 ? amount / quantity : 0;
            }
            else if (inv.Type == Inventories.InventoriesType.Sales && !string.IsNullOrEmpty(inv.SalesItemsId))
            {
                var salesItem = allSalesItems.FirstOrDefault(si => si.Id == inv.SalesItemsId);
                if (salesItem != null)
                {
                    var sd = salesDetails.FirstOrDefault(s => s.Id == salesItem.SalesDetailsId);
                    billNumber = sd?.BillNumber;
                }

                quantity = inv.QuantityOut;
                amount = inv.AmountOut;
                price = quantity != 0 ? amount / quantity : 0;
            }

            return new GetItemRateHistoryQueryResponse(
                date: inv.EntryDate,
                type: inv.Type,
                billNumber: billNumber,
                ledgerId: inv.LedgerId,
                itemId: inv.ItemId,
                itemGroupId: inv.Items?.ItemGroupId,
                unitId: inv.Items?.UnitId,
                quantity: quantity,
                price: price,
                amount: amount
            );
        })
        .OrderBy(r => r.date)
        .ToList();


                PagedResult<GetItemRateHistoryQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = inventoryRecords.Count();

                    var pagedItems = inventoryRecords
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetItemRateHistoryQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetItemRateHistoryQueryResponse>
                    {
                        Items = inventoryRecords.ToList(),
                        TotalItems = inventoryRecords.Count(),
                        PageIndex = 1,
                        pageSize = inventoryRecords.Count()
                    };
                }


                return Result<PagedResult<GetItemRateHistoryQueryResponse>>.Success(finalResponseList);
    }
    catch (Exception ex)
    {
        return Result<PagedResult<GetItemRateHistoryQueryResponse>>.Failure(
            $"Error fetching item rate history: {ex.Message}");
    }
        }

        public async Task<Result<PagedResult<GetItemwiseProfitQueryResponse>>> GetItemwiseProfit(PaginationRequest paginationRequest, ItemwiseProfitDtos itemwiseProfitDtos, CancellationToken cancellationToken = default)
        {
            try
            {
              
                var (salesQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<SalesDetails>();

                IQueryable<SalesDetails> filteredSales = salesQuery;

              
                if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                    filteredSales = filteredSales.Where(x => x.CompanyId == currentCompanyId);
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );
                    filteredSales = filteredSales.Where(x => companyIds.Contains(x.CompanyId));
                }

                filteredSales = filteredSales.Where(s => s.Status != SalesStatus.Returned && s.IsSales == true);

                
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (!string.IsNullOrWhiteSpace(itemwiseProfitDtos.startDate))
                    startDate = await _dateConvertHelper.ConvertToEnglish(itemwiseProfitDtos.startDate.Trim());

                if (!string.IsNullOrWhiteSpace(itemwiseProfitDtos.endDate))
                    endDate = await _dateConvertHelper.ConvertToEnglish(itemwiseProfitDtos.endDate.Trim());

                if (startDate.HasValue)
                    filteredSales = filteredSales.Where(s => s.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    filteredSales = filteredSales.Where(s => s.CreatedAt <= endDate.Value);

               
                var salesData = await filteredSales
                    .Include(s => s.SalesItems)
                        .ThenInclude(si => si.Item)
                    .OrderByDescending(s => s.CreatedAt)
                    .ToListAsync(cancellationToken);

                
                var itemwiseProfit = salesData
                    .SelectMany(s => s.SalesItems)
                    .Where(si =>
                        (string.IsNullOrWhiteSpace(itemwiseProfitDtos.stockCenterId) || si.Item.StockCenterId == itemwiseProfitDtos.stockCenterId) &&
                        (string.IsNullOrWhiteSpace(itemwiseProfitDtos.itemId) || si.ItemId == itemwiseProfitDtos.itemId) &&
                        (string.IsNullOrWhiteSpace(itemwiseProfitDtos.itemGroupId) || si.Item.ItemGroupId == itemwiseProfitDtos.itemGroupId)
                    )
                    .Select(si =>
                    {
                        decimal costPrice = 0m;
                        if (!string.IsNullOrEmpty(si.Item.CostPrice))
                            decimal.TryParse(si.Item.CostPrice, out costPrice);

                        return new
                        {
                            si.Item.StockCenterId,
                            si.ItemId,
                            si.Item.ItemGroupId,
                            si.Item.UnitId,
                            SalesQty = si.Quantity,
                            SellingPrice = si.Price,
                            si.Amount,
                            CostPrice = costPrice
                        };
                    })
                    .GroupBy(x => new { x.StockCenterId, x.ItemId, x.ItemGroupId, x.UnitId })
                    .Select(g =>
                    {
                        var totalQty = g.Sum(x => x.SalesQty);
                        var totalAmount = g.Sum(x => x.Amount);
                        var totalCost = g.Sum(x => x.SalesQty * x.CostPrice);
                        var grossProfit = totalAmount - totalCost;
                        var grossProfitRate = totalCost > 0 ? (grossProfit / totalAmount) * 100 : 0;

                        return new GetItemwiseProfitQueryResponse(
                            g.Key.StockCenterId ?? "",
                            g.Key.ItemId,
                            g.Key.ItemGroupId,
                            g.Key.UnitId,
                            totalQty,
                            g.Average(x => x.SellingPrice),
                            totalAmount,
                            grossProfit,
                            grossProfitRate
                        );
                    });


                PagedResult<GetItemwiseProfitQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = itemwiseProfit.Count();

                    var pagedItems = itemwiseProfit
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetItemwiseProfitQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetItemwiseProfitQueryResponse>
                    {
                        Items = itemwiseProfit.ToList(),
                        TotalItems = itemwiseProfit.Count(),
                        PageIndex = 1,
                        pageSize = itemwiseProfit.Count()
                    };
                }


                return Result<PagedResult<GetItemwiseProfitQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetItemwiseProfitQueryResponse>>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>> GetItemwisePurchaseByExpireDate(PaginationRequest paginationRequest, ItemwisePurchaseExpireDateDtos itemwisePurchaseExpireDateDtos, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Get scoped data (user, company, etc.)
                var (purchaseQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();

                IQueryable<PurchaseDetails> filteredPurchase = purchaseQuery;

                // 2. Filter by company scope if needed
                if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filteredPurchase = filteredPurchase.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filteredPurchase = filteredPurchase.Where(x => companyIds.Contains(x.CompanyId));
                }

                filteredPurchase = filteredPurchase.Where(s => s.Status != PurchaseStatus.Returned);


                //int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                //int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                //var totalItems = await filteredPurchase.CountAsync();

                var pagedPurchases = await filteredPurchase
                    .Include(s => s.PurchaseItems)
                        .ThenInclude(pi => pi.Item)
                    .OrderByDescending(s => s.CreatedAt)
                    //.Skip((pageIndex - 1) * pageSize)
                    //.Take(pageSize)
                    .SelectMany(s => s.PurchaseItems.Select(pi => new GetItemwisePurchaseExpireDateResponse(
                        pi.ItemId,
                        s.Date ?? "",
                        pi.Item.ItemGroupId,
                        null,
                        s.BillNumber ?? "",
                        s.ReferenceNumber ?? "",
                        s.LedgerId ?? "",
                        pi.Quantity,
                        pi.Price,
                        pi.Amount,
                        s.DiscountAmount ?? 0,
                        s.VatAmount ?? 0,
                        s.GrandTotalAmount,
                        s.StockCenterId ?? ""
                    )))
                    .ToListAsync(cancellationToken);


                PagedResult<GetItemwisePurchaseExpireDateResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = pagedPurchases.Count();

                    var pagedItems = pagedPurchases
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetItemwisePurchaseExpireDateResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetItemwisePurchaseExpireDateResponse>
                    {
                        Items = pagedPurchases.ToList(),
                        TotalItems = pagedPurchases.Count(),
                        PageIndex = 1,
                        pageSize = pagedPurchases.Count()
                    };
                }


                return Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>.Failure(ex.Message);
            }
        }

        public async Task<Result<PagedResult<ItemwisePurchaseReportQueryResponse>>> GetItemwisePurchaseReport(PaginationRequest paginationRequest, ItemwisePurchaseReportDto itemwisePurchaseReportDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (purchaseDetailsQueryable, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();

                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();

                var filterPurchase = isSuperAdmin
                    ? purchaseDetailsQueryable
                    : purchaseDetailsQueryable.Where(x => x.CompanyId == currentCompanyId);

                // Date filters
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(itemwisePurchaseReportDto.startDate.Trim());
                    if (DateTime.TryParse(itemwisePurchaseReportDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(itemwisePurchaseReportDto.endDate.Trim());
                    if (DateTime.TryParse(itemwisePurchaseReportDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }

           
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

                var userId = _tokenService.GetUserId;

                var purchaseResult = await filterPurchase
                    .Where(x =>
                        x.CreatedBy == userId() &&
                        (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                        (endDateUtc == null || x.CreatedAt < endDateUtc) &&
                    (string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.ledgerId) || x.LedgerId == itemwisePurchaseReportDto.ledgerId) &&
                    (string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.stockCenterId) || x.StockCenterId == itemwisePurchaseReportDto.stockCenterId))
                .Include(sd => sd.PurchaseItems)
                .ThenInclude(si => si.Item)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var responseList = purchaseResult
                    .SelectMany(sd => sd.PurchaseItems.Select(si => new ItemwisePurchaseReportQueryResponse(
                        sd.Date,
                        sd.BillNumber,
                        sd.ReferenceNumber,
                        sd.LedgerId,
                        si.ItemId,
                        si.Item?.ItemGroupId,
                        si.UnitId,
                        si.Price,
                        si.Quantity,
                        si.Amount,
                        sd.DiscountAmount,
                        sd.VatAmount,
                        sd.GrandTotalAmount,
                        sd.Status,
                        sd.StockCenterId ?? ""
                    )))
                .Where(r =>
                (string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.itemId) || r.itemId == itemwisePurchaseReportDto.itemId) &&
                (string.IsNullOrWhiteSpace(itemwisePurchaseReportDto.itemGroupId) || r.itemGroupId == itemwisePurchaseReportDto.itemGroupId)
                )
                    .ToList();

                PagedResult<ItemwisePurchaseReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<ItemwisePurchaseReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<ItemwisePurchaseReportQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }
                return Result<PagedResult<ItemwisePurchaseReportQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Itemwise Sales Report: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<PurchaseReturnDayBookQueryResponse>>> GetPurchaseReturnDayBook(PaginationRequest paginationRequest, PurchaseReturnDayBookDto purchaseReturnDayBookDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (ledger, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseReturnDetails>();


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

                if (!string.IsNullOrWhiteSpace(purchaseReturnDayBookDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReturnDayBookDto.startDate.Trim());

                    if (DateTime.TryParse(purchaseReturnDayBookDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }


                if (!string.IsNullOrWhiteSpace(purchaseReturnDayBookDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReturnDayBookDto.endDate.Trim());

                    if (DateTime.TryParse(purchaseReturnDayBookDto.endDate, out var tempEnd) &&
                        tempEnd.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = endDateUtc?.AddDays(1);
                    }
                }


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

                var userId = _tokenService.GetUserId;


                var purchaseReturnDayBookResult = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                    .GetConditionalAsync(
                        predicate: x =>
                            x.CreatedBy == userId() &&

                            (startDateUtc == null || x.CreatedAt >= startDateUtc) &&
                            (endDateUtc == null || x.CreatedAt < endDateUtc),
                        queryModifier: q => q.AsNoTracking().Include(pr=>pr.PurchaseDetails)
                    );


                var responseList = purchaseReturnDayBookResult.Select(pr => new PurchaseReturnDayBookQueryResponse(
                         pr.ReturnDate,
                         pr.PurchaseDetails?.BillNumber ?? "",
                         pr.LedgerId,
                         pr.NetReturnAmount,
                         pr.TaxAdjustment,
                         pr.TotalReturnAmount

                    )).ToList();

                PagedResult<PurchaseReturnDayBookQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<PurchaseReturnDayBookQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<PurchaseReturnDayBookQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count()
                    };
                }

                return Result<PagedResult<PurchaseReturnDayBookQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching Purchase Return day book: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetPurchaseReturnReportQueryResponse>>> GetPurchaseReturnReport(PaginationRequest paginationRequest, PurchaseReturnReportDto purchaseReturnReportDto, CancellationToken cancellationToken = default)
        {

            try
            {
                var (purchaseReturn, companyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseReturnDetails>();

              
                var currentCompanyId = _tokenService.CompanyId().FirstOrDefault();
                // Filter based on user scope
                var filteredLedger = isSuperAdmin
                    ? purchaseReturn
                    : purchaseReturn.Where(x => x.CompanyId == _tokenService.CompanyId().FirstOrDefault() || string.IsNullOrEmpty(x.CompanyId));

                // Convert start and end dates
                DateTime? startDateUtc = null;
                DateTime? endDateUtc = null;

                if (!string.IsNullOrWhiteSpace(purchaseReturnReportDto.startDate?.Trim()))
                {
                    startDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReturnReportDto.startDate.Trim());
                    if (DateTime.TryParse(purchaseReturnReportDto.startDate, out var tempStart) &&
                        tempStart.TimeOfDay == TimeSpan.Zero)
                    {
                        endDateUtc = startDateUtc?.AddDays(1);
                    }
                }

                if (!string.IsNullOrWhiteSpace(purchaseReturnReportDto.endDate?.Trim()))
                {
                    endDateUtc = await _dateConvertHelper.ConvertToEnglish(purchaseReturnReportDto.endDate.Trim());
                    if (DateTime.TryParse(purchaseReturnReportDto.endDate, out var tempEnd) &&
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

                // Fetch data with filters and includes
                var purchaseReturnReportResult = await _unitOfWork.BaseRepository<PurchaseReturnDetails>()
                    .GetConditionalAsync(
                        predicate: pr =>
                    pr.CreatedBy == userId() &&
                    (startDateUtc == null || pr.CreatedAt >= startDateUtc) &&
                    (endDateUtc == null || pr.CreatedAt < endDateUtc) &&
                    (string.IsNullOrWhiteSpace(purchaseReturnReportDto.ledgerId) || pr.LedgerId == purchaseReturnReportDto.ledgerId) &&
                    (string.IsNullOrWhiteSpace(purchaseReturnReportDto.stockCenterId) || pr.PurchaseDetails.StockCenterId == purchaseReturnReportDto.stockCenterId) &&
                    (isSuperAdmin || pr.PurchaseDetails.CompanyId == currentCompanyId),  
                queryModifier: q => q
                    .AsNoTracking()
                    .Include(pr => pr.PurchaseReturnItems)
                        .ThenInclude(pri => pri.PurchaseItems)
                            .ThenInclude(pi => pi.Item)
                                .ThenInclude(i => i.ItemGroup)
                    .Include(pr => pr.PurchaseDetails)
                    );


                var responseList = purchaseReturnReportResult
                    .SelectMany(pr => pr.PurchaseReturnItems
                    .Where(pri =>
                    (string.IsNullOrWhiteSpace(purchaseReturnReportDto.purchaseItemsId) || pri.PurchaseItemsId == purchaseReturnReportDto.purchaseItemsId) &&
                    (string.IsNullOrWhiteSpace(purchaseReturnReportDto.itemGroupId) || pri.PurchaseItems.Item.ItemGroupId == purchaseReturnReportDto.itemGroupId)
                    )
                    .Select(sri => new GetPurchaseReturnReportQueryResponse(
                            pr.ReturnDate,
                            pr.PurchaseReturnNumber ?? "",
                            pr.LedgerId ?? "",
                            sri.PurchaseItemsId,
                            sri.PurchaseItems.Item.ItemGroupId ?? "",
                            sri.PurchaseItems.UnitId,
                            sri.ReturnQuantity,
                            sri.ReturnUnitPrice,
                            pr.NetReturnAmount,
                            pr.TaxAdjustment,
                            pr.TotalReturnAmount,
                            pr.StockCenterId

                        ))
                    )
                    .ToList();

                PagedResult<GetPurchaseReturnReportQueryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = responseList.Count();

                    var pagedItems = responseList
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetPurchaseReturnReportQueryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetPurchaseReturnReportQueryResponse>
                    {
                        Items = responseList.ToList(),
                        TotalItems = responseList.Count(),
                        PageIndex = 1,
                        pageSize = responseList.Count() // all items in one page
                    };
                }
                return Result<PagedResult<GetPurchaseReturnReportQueryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching PurchaseReturn Report: {ex.Message}", ex);
            }
        }

        public async Task<Result<PagedResult<GetPurchaseSummaryResponse>>> PurchaseSummaryReport(PaginationRequest paginationRequest, PurchaseSummaryDtos purchaseSummaryDtos, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Get scoped purchase details query
                var (purchaseQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<PurchaseDetails>();

                IQueryable<PurchaseDetails> filteredPurchase = purchaseQuery;

               
                if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filteredPurchase = filteredPurchase.Where(x => x.CompanyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filteredPurchase = filteredPurchase.Where(x => companyIds.Contains(x.CompanyId));
                }

                var topItem = await filteredPurchase
               .SelectMany(s => s.PurchaseItems)
               .GroupBy(si => new { si.Id, si.Item.Name })
               .Select(g => new
               {
                   g.Key.Id,
                   g.Key.Name,
                   TotalQuantity = g.Sum(x => x.Quantity)
               })
               .OrderByDescending(g => g.TotalQuantity)
               .Select(g => g.Name)
               .FirstOrDefaultAsync(cancellationToken) ?? "";
                var query = filteredPurchase
                    .SelectMany(pd => pd.PurchaseItems.Select(pi => new
                    {
                        pd.Id,
                        pd.BillNumber,
                        pd.LedgerId,
                        pd.StockCenterId,
                        pd.DiscountAmount,
                        pd.VatAmount,
                        pd.GrandTotalAmount,
                        pi.ItemId,
                        ItemName = pi.Item.Name,
                        pi.Quantity,
                        pi.Price,
                        pi.Amount
                    }));

                // 3. Apply filters from DTO
                if (!string.IsNullOrEmpty(purchaseSummaryDtos.itemId))
                    query = query.Where(x => x.ItemId == purchaseSummaryDtos.itemId);

                if (!string.IsNullOrEmpty(purchaseSummaryDtos.ledgerId))
                    query = query.Where(x => x.LedgerId == purchaseSummaryDtos.ledgerId);

                if (!string.IsNullOrEmpty(purchaseSummaryDtos.stockCenterId))
                    query = query.Where(x => x.StockCenterId == purchaseSummaryDtos.stockCenterId);

                // 4. Group by Item/Ledger/StockCenter
                var grouped = await query
                    .GroupBy(x => new { x.ItemId, x.LedgerId, x.StockCenterId })
                    .Select(g => new GetPurchaseSummaryResponse(
                       
                        g.Key.ItemId,
                        g.Key.LedgerId,
                        g.Key.StockCenterId,
                        g.Select(x => x.BillNumber).Distinct().Count(),  
                        g.Sum(x => x.Quantity),                          
                        g.Sum(x => x.Amount),                           
                        g.Sum(x => x.DiscountAmount ?? 0),               
                        g.Sum(x => x.VatAmount ?? 0),                  
                        g.Sum(x => x.GrandTotalAmount),                  
                        topItem
                    ))
                    .ToListAsync(cancellationToken);


                PagedResult<GetPurchaseSummaryResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = grouped.Count();

                    var pagedItems = grouped
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetPurchaseSummaryResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetPurchaseSummaryResponse>
                    {
                        Items = grouped.ToList(),
                        TotalItems = grouped.Count(),
                        PageIndex = 1,
                        pageSize = grouped.Count() // all items in one page
                    };
                }

                return Result<PagedResult<GetPurchaseSummaryResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetPurchaseSummaryResponse>>.Failure(ex.Message);
            }
        }
  
    
    
    }
}