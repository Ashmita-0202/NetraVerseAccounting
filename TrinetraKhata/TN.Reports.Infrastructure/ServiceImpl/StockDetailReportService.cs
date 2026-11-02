using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TN.Authentication.Domain.Entities;
using TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Reports.Application.ItemwiseSalesReport;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.SalesSummaryReport;
using TN.Reports.Application.ServiceInterface;
using TN.Reports.Application.StockDetailReport.Queries;
using TN.Reports.Application.StockReport.Queries;
using TN.Sales.Domain.Entities;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.Entities.Inventory;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using TN.Shared.Domain.IRepository;
using TN.Shared.Infrastructure.Repository;
using static TN.Shared.Domain.Entities.Transactions.TransactionDetail;

namespace TN.Reports.Infrastructure.ServiceImpl
{
    public class StockDetailReportService : IStockDetailReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Inventories> _inventoriesRepository;
        private readonly IBaseRepository<PurchaseDetails> _purchaseRepository;
        private readonly IBaseRepository<PurchaseReturnDetails> _purchaseReturnRepository;
        private readonly IBaseRepository<SalesDetails> _salesRepository;
        private readonly IBaseRepository<SalesReturnDetails> _salesReturnRepository;
        private readonly IGetUserScopedData _getUserScopedData;

        public StockDetailReportService
            (
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IGetUserScopedData getUserScopedData,
            IMapper mapper,
            IBaseRepository<Inventories> inventoriesRepository,
            IBaseRepository<PurchaseDetails> purchaseRepository,
            IBaseRepository<PurchaseReturnDetails> purchaseReturnRepository,
            IBaseRepository<SalesDetails> salesRepository,
            IBaseRepository<SalesReturnDetails> salesReturnRepository
            )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _inventoriesRepository = inventoriesRepository;
            _purchaseRepository = purchaseRepository;
            _purchaseReturnRepository = purchaseReturnRepository;
            _salesRepository = salesRepository;
            _salesReturnRepository = salesReturnRepository;
            _getUserScopedData = getUserScopedData;

        }
        public async Task<Result<PagedResult<GetStockDetailReportResponse>>> GetStockDetailReport(string companyId,PaginationRequest paginationRequest,FilterStockDetailReportDto filterStockDetailReportDto, CancellationToken cancellationToken = default)
        {
            try
            {
             

                #region Filter in Old Ways
              // 🔹 Opening stock
              //var openingStock = await _inventoriesRepository.GetConditionalAsync(
              //    predicate: i => i.IsOpeningStuck == true && i.companyId == companyId,
              //    queryModifier: q => q.Include(i => i.Items)
              //);

              //// 🔹 Purchases
              //var purchases = await _purchaseRepository.GetConditionalAsync(
              //    predicate: i=>i.CompanyId == companyId,
              //    queryModifier: q => q.Include(pd => pd.PurchaseItems).ThenInclude(pi => pi.Item)
              //);

              //// 🔹 Purchase returns
              //var purchaseReturns = await _purchaseReturnRepository.GetConditionalAsync(
              //     predicate: i => i.CompanyId == companyId,
              //    queryModifier: q => q.Include(pr => pr.PurchaseReturnItems)
              //                         .ThenInclude(pri => pri.PurchaseItems)
              //                         .ThenInclude(pi => pi.Item)
              //);

              //// 🔹 Sales
              //var sales = await _salesRepository.GetConditionalAsync(
              //     predicate: i => i.CompanyId == companyId,
              //    queryModifier: q => q.Include(sd => sd.SalesItems).ThenInclude(si => si.Item)
              //);

              //// 🔹 Sales returns
              //var salesReturns = await _salesReturnRepository.GetConditionalAsync(
              //     predicate: i => i.CompanyId == companyId,
              //    queryModifier: q => q.Include(sr => sr.SalesReturnItems)
              //                         .ThenInclude(sri => sri.SalesItems)
              //                         .ThenInclude(si => si.Item)
              //);

              //// 🔹 Group purchase returns by ItemId
              //var purchaseReturnGrouped = purchaseReturns
              //    .SelectMany(pr => pr.PurchaseReturnItems)
              //    .GroupBy(x => x.PurchaseItems.ItemId)
              //    .ToDictionary(
              //        g => g.Key,
              //        g => new
              //        {
              //            Quantity = g.Sum(x => x.ReturnQuantity),
              //            Amount = g.Sum(x => x.ReturnTotalAmount),
              //            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
              //                ? g.Sum(x => x.ReturnTotalAmount) / g.Sum(x => x.ReturnQuantity)
              //                : 0
              //        });

              //// 🔹 Group sales returns by ItemId
              //var salesReturnGrouped = salesReturns
              //    .SelectMany(sr => sr.SalesReturnItems)
              //    .GroupBy(x => x.SalesItems.ItemId)
              //    .ToDictionary(
              //        g => g.Key,
              //        g => new
              //        {
              //            Quantity = g.Sum(x => x.ReturnQuantity),
              //            Amount = g.Sum(x => x.ReturnTotalPrice),
              //            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
              //                ? g.Sum(x => x.ReturnTotalPrice) / g.Sum(x => x.ReturnQuantity)
              //                : 0
              //        });

              //// 🔹 Build stock report
              //var grouped = purchases
              //    .SelectMany(pd => pd.PurchaseItems)
              //    .GroupBy(pi => new { pi.ItemId, pi.Item.Name, pi.Item.UnitId })
              //    .Select(g =>
              //    {
              //        var itemId = g.Key.ItemId;

              //        // Opening stock
              //        var openingEntries = openingStock.Where(i => i.ItemId == itemId).ToList();
              //        var openingQty = openingEntries.Sum(x => x.QuantityIn);
              //        var openingAmt = openingEntries.Sum(x => x.AmountIn);
              //        var openingAvgPrice = openingQty != 0 ? openingAmt / openingQty : 0;

              //        // Purchases
              //        var totalPurchaseQty = g.Sum(x => x.Quantity);
              //        var totalPurchaseAmt = g.Sum(x => x.Quantity * x.Price);
              //        var avgPurchasePrice = totalPurchaseQty != 0 ? totalPurchaseAmt / totalPurchaseQty : 0;

              //        // Sales
              //        var salesGrouped = sales.SelectMany(s => s.SalesItems)
              //                                .Where(si => si.ItemId == itemId)
              //                                .ToList();
              //        var totalSalesQty = salesGrouped.Sum(x => x.Quantity);
              //        var totalSalesAmt = salesGrouped.Sum(x => x.Quantity * x.Price);
              //        var avgSalesPrice = totalSalesQty != 0 ? totalSalesAmt / totalSalesQty : 0;

              //        // Purchase returns
              //        purchaseReturnGrouped.TryGetValue(itemId, out var pr);

              //        // Sales returns
              //        salesReturnGrouped.TryGetValue(itemId, out var sr);

              //        return new GetStockDetailReportResponse(
              //            ItemName: g.Key.Name ?? string.Empty,
              //            UnitId: g.Key.UnitId ?? string.Empty,
              //            OpeningStockQty: openingQty,
              //            OpeningStockAvgPrice: openingAvgPrice,
              //            OpeningStockAmount: openingAmt,
              //            PurchaseQty: totalPurchaseQty,
              //            PurchaseAvgPrice: avgPurchasePrice,
              //            PurchaseAmountIn: totalPurchaseAmt,
              //            SalesQty: totalSalesQty,
              //            SalesAvgPrice: avgSalesPrice,
              //            SalesAmountOut: totalSalesAmt,
              //            PurchaseReturnQty: pr?.Quantity ?? 0,
              //            PurchaseReturnAvgPrice: pr?.AvgPrice ?? 0,
              //            PurchaseReturnAmount: pr?.Amount ?? 0,
              //            SalesReturnQty: sr?.Quantity ?? 0,
              //            SalesReturnAvgPrice: sr?.AvgPrice ?? 0,
              //            SalesReturnAmount: sr?.Amount ?? 0
              //        );
              //    })
              //    .ToList();

               #endregion

              // 🔹 Opening stock
              var openingStock = await _inventoriesRepository.GetConditionalAsync(

                    predicate: i => i.IsOpeningStuck == true && i.companyId == companyId,
                    queryModifier: q => q.Include(i => i.Items) 
                );

                // 🔹 Purchases
                var purchases = await _purchaseRepository.GetConditionalAsync(
                    predicate: i => i.CompanyId == companyId,
                    queryModifier: q => q.Include(pd => pd.PurchaseItems).ThenInclude(pi => pi.Item)
                );

                // 🔹 Purchase returns
                var purchaseReturns = await _purchaseReturnRepository.GetConditionalAsync(
                     predicate: i => i.CompanyId == companyId,
                    queryModifier: q => q.Include(pr => pr.PurchaseReturnItems)
                                         .ThenInclude(pri => pri.PurchaseItems)
                                         .ThenInclude(pi => pi.Item)
                );

                // 🔹 Sales
                var sales = await _salesRepository.GetConditionalAsync(
                     predicate: i => i.CompanyId == companyId,
                    queryModifier: q => q.Include(sd => sd.SalesItems).ThenInclude(si => si.Item)
                );

                // 🔹 Sales returns
                var salesReturns = await _salesReturnRepository.GetConditionalAsync(
                     predicate: i => i.CompanyId == companyId,
                    queryModifier: q => q.Include(sr => sr.SalesReturnItems)
                                         .ThenInclude(sri => sri.SalesItems)
                                         .ThenInclude(si => si.Item)
                );

                // 🔹 Group purchase returns by ItemId
                var purchaseReturnGrouped = purchaseReturns
                    .SelectMany(pr => pr.PurchaseReturnItems)
                    .GroupBy(x => x.PurchaseItems.ItemId)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Quantity = g.Sum(x => x.ReturnQuantity),
                            Amount = g.Sum(x => x.ReturnTotalAmount),
                            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
                                ? g.Sum(x => x.ReturnTotalAmount) / g.Sum(x => x.ReturnQuantity)
                                : 0
                        });

                // 🔹 Group sales returns by ItemId
                var salesReturnGrouped = salesReturns
                    .SelectMany(sr => sr.SalesReturnItems)
                    .GroupBy(x => x.SalesItems.ItemId)
                    .ToDictionary(
                        g => g.Key,
                        g => new
                        {
                            Quantity = g.Sum(x => x.ReturnQuantity),
                            Amount = g.Sum(x => x.ReturnTotalPrice),
                            AvgPrice = g.Sum(x => x.ReturnQuantity) != 0
                                ? g.Sum(x => x.ReturnTotalPrice) / g.Sum(x => x.ReturnQuantity)
                                : 0
                        });

                // 🔹 Collect all item IDs from all sources
                var allItemIds = openingStock.Select(i => i.ItemId)
                    .Union(purchases.SelectMany(p => p.PurchaseItems).Select(pi => pi.ItemId))
                    .Union(sales.SelectMany(s => s.SalesItems).Select(si => si.ItemId))
                    .Union(purchaseReturnGrouped.Keys)   // ✅ corrected
                    .Union(salesReturnGrouped.Keys)      // ✅ corrected
                    .Distinct()
                    .ToList();

                // 🔹 Build report for each item
                var grouped = allItemIds.Select(itemId =>
                {
                    // Get item info (from purchases or any available source)
                    var item = purchases.SelectMany(p => p.PurchaseItems)
                                        .Select(pi => pi.Item)
                                        .FirstOrDefault(it => it.Id == itemId)
                               ?? sales.SelectMany(s => s.SalesItems)
                                        .Select(si => si.Item)
                                        .FirstOrDefault(it => it.Id == itemId)
                               ?? openingStock.FirstOrDefault(o => o.ItemId == itemId)?.Items;

                    var itemsId = item?.Id ?? "Unknown";
                    var unitId = item?.UnitId ?? string.Empty;

                    // Opening stock
                    var openingEntries = openingStock.Where(i => i.ItemId == itemId).ToList();
                    var openingQty = openingEntries.Sum(x => x.QuantityIn);
                    var openingAmt = openingEntries.Sum(x => x.AmountIn);
                    var openingAvgPrice = openingQty != 0 ? openingAmt * openingQty / openingQty : 0;

                    // Purchases
                    var purchaseItems = purchases.SelectMany(p => p.PurchaseItems)
                                                 .Where(pi => pi.ItemId == itemId)
                                                 .ToList();
                    var totalPurchaseQty = purchaseItems.Sum(x => x.Quantity);
                    var totalPurchaseAmt = purchaseItems.Sum(x => x.Quantity * x.Price);
                    var avgPurchasePrice = totalPurchaseQty != 0 ? totalPurchaseAmt * totalPurchaseQty / totalPurchaseQty : 0;

                    // Sales
                    var salesItems = sales.SelectMany(s => s.SalesItems)
                                          .Where(si => si.ItemId == itemId)
                                          .ToList();
                    var totalSalesQty = salesItems.Sum(x => x.Quantity);
                    var totalSalesAmt = salesItems.Sum(x => x.Quantity * x.Price);
                    var avgSalesPrice = totalSalesQty != 0 ? totalSalesAmt * totalSalesQty / totalSalesQty : 0;

                    // Purchase returns
                    purchaseReturnGrouped.TryGetValue(itemId, out var pr);

                    // Sales returns
                    salesReturnGrouped.TryGetValue(itemId, out var sr);



                    var balanceQty = (openingQty + totalPurchaseQty + (sr?.Quantity ?? 0)) - (totalSalesQty + (pr?.Quantity ?? 0));
                    var balanceAvgPrice = balanceQty != 0 ? ((openingAmt + totalPurchaseAmt + (sr?.Amount ?? 0)) - (totalSalesAmt + (pr?.Amount ?? 0))) * balanceQty / balanceQty : 0;
                    var balanceAmt = (openingAmt + totalPurchaseAmt + (sr?.Amount ?? 0)) - (totalSalesAmt + (pr?.Amount ?? 0));
                    return new GetStockDetailReportResponse(
                        itemId: itemsId,
                        UnitId: unitId,
                        OpeningStockQty: openingQty,
                        OpeningStockAvgPrice: openingAvgPrice,
                        OpeningStockAmount: openingAmt,
                        PurchaseQty: totalPurchaseQty,
                        PurchaseAvgPrice: avgPurchasePrice,
                        PurchaseAmountIn: totalPurchaseAmt,
                        SalesQty: totalSalesQty,
                        SalesAvgPrice: avgSalesPrice,
                        SalesAmountOut: totalSalesAmt,
                        PurchaseReturnQty: pr?.Quantity ?? 0,
                        PurchaseReturnAvgPrice: pr?.AvgPrice ?? 0,
                        PurchaseReturnAmount: pr?.Amount ?? 0,
                        SalesReturnQty: sr?.Quantity ?? 0,
                        SalesReturnAvgPrice: sr?.AvgPrice ?? 0,
                        SalesReturnAmount: sr?.Amount ?? 0,
                        BalanceQty: balanceQty,
                        BalanceAvgPrice: balanceAvgPrice,
                        BalanceAmount: balanceAmt,
                        stockAdjustmentValue: "N/A" // Placeholder
                    );
                })
            // 🔹 Filter out items where everything is 0 or less
            .Where(x =>
                (x.OpeningStockQty > 0 || x.OpeningStockAmount > 0 || x.OpeningStockAvgPrice > 0) ||
                (x.PurchaseQty > 0 || x.PurchaseAmountIn > 0 || x.PurchaseAvgPrice > 0) ||
                (x.SalesQty > 0 || x.SalesAmountOut > 0 || x.SalesAvgPrice > 0) ||
                (x.PurchaseReturnQty > 0 || x.PurchaseReturnAmount > 0 || x.PurchaseReturnAvgPrice > 0) ||
                (x.SalesReturnQty > 0 || x.SalesReturnAmount > 0 || x.SalesReturnAvgPrice > 0) ||
                (x.BalanceQty > 0 || x.BalanceAmount > 0 || x.BalanceAvgPrice > 0)
            )
            .ToList();
                if (!string.IsNullOrWhiteSpace(filterStockDetailReportDto?.itemId))
                {
                    grouped = grouped
                        .Where(x => x.itemId != "Unknown" && x.itemId != null) 
                        .Where(x => x.itemId == filterStockDetailReportDto.itemId) 
                        .ToList();
                }


                PagedResult<GetStockDetailReportResponse> finalResponseList;

                if (paginationRequest.IsPagination)
                {

                    int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                    int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                    int totalItems = grouped.Count();

                    var pagedItems = grouped
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    finalResponseList = new PagedResult<GetStockDetailReportResponse>
                    {
                        Items = pagedItems,
                        TotalItems = totalItems,
                        PageIndex = pageIndex,
                        pageSize = pageSize
                    };
                }
                else
                {
                    finalResponseList = new PagedResult<GetStockDetailReportResponse>
                    {
                        Items = grouped.ToList(),
                        TotalItems = grouped.Count(),
                        PageIndex = 1,
                        pageSize = grouped.Count()
                    };
                }
                return Result<PagedResult<GetStockDetailReportResponse>>.Success(finalResponseList);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetStockDetailReportResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<Result<PagedResult<GetStockReportQueryResponse>>> GetStockReport(PaginationRequest paginationRequest, StockReportDtos stockReportDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var (inventoryQuery, currentCompanyId, institutionId, userRole, isSuperAdmin) =
                    await _getUserScopedData.GetUserScopedData<Inventories>();

                IQueryable<Inventories> filteredStock = inventoryQuery
                    .Include(i => i.Items)
                        .ThenInclude(x => x.ItemInstances);

                if (!string.IsNullOrEmpty(stockReportDto.companyId))
                {
                    filteredStock = filteredStock.Where(x => x.companyId == stockReportDto.companyId);
                }
                else if (!string.IsNullOrEmpty(currentCompanyId) && !isSuperAdmin)
                {
                    filteredStock = filteredStock.Where(x => x.companyId == currentCompanyId);
                }
                else if (!string.IsNullOrEmpty(institutionId))
                {
                    var companyIds = await _unitOfWork.BaseRepository<Company>()
                        .GetConditionalFilterType(
                            x => x.InstitutionId == institutionId,
                            query => query.Select(c => c.Id)
                        );

                    filteredStock = filteredStock.Where(x => companyIds.Contains(x.companyId));
                }

                // ✅ Apply Filter Conditions
                filteredStock = filteredStock.Where(s =>
                     (string.IsNullOrEmpty(stockReportDto.stockCenterId) || s.StockCenterId == stockReportDto.stockCenterId) &&
                     (string.IsNullOrEmpty(stockReportDto.itemGroupId) || s.Items.ItemGroupId == stockReportDto.itemGroupId) &&
                     (string.IsNullOrEmpty(stockReportDto.itemId) || s.ItemId == stockReportDto.itemId) &&
                     (string.IsNullOrEmpty(stockReportDto.ledgerId) || s.LedgerId == stockReportDto.ledgerId) &&
                    (stockReportDto.batchNumbers == null || !stockReportDto.batchNumbers.Any() ||
                     (s.Items.BatchNumbers != null && s.Items.BatchNumbers.Any(b => stockReportDto.batchNumbers.Contains(b.BatchNumbers)))) &&
                   (stockReportDto.SerialNumbers == null || !stockReportDto.SerialNumbers.Any() ||
                  (s.Items.ItemInstances != null && s.Items.ItemInstances.Any(i => stockReportDto.SerialNumbers.Contains(i.SerialNumber)))) &&
                   (stockReportDto.manufactureAndExpiries == null || !stockReportDto.manufactureAndExpiries.Any() ||
                        (s.Items.ManufacturingAndExpiries != null &&
                         s.Items.ManufacturingAndExpiries.Any(m => m.ExpiredDate.HasValue && stockReportDto.manufactureAndExpiries.Contains(m.ExpiredDate.Value))))
                                       
                   
                
                );

                var purchaseDetails = await _purchaseRepository.GetAllAsync();
                var referenceNumber = purchaseDetails
                 .FirstOrDefault(p => p.PurchaseItems.Any(pi => pi.ItemId == pi.ItemId))?.ReferenceNumber ?? "";

                // ✅ Project Result
                var stockList = await filteredStock
                    .OrderByDescending(s => s.CreatedAt)
                    .Select(x => new GetStockReportQueryResponse(
                        x.LedgerId,
                        x.StockCenters.Name ?? "",
                        x.ItemId ?? "",
                        x.Items.ItemGroupId ?? "",
                        x.UnitId ?? "",
                        (x.QuantityIn - x.QuantityOut),
                        Convert.ToDecimal(x.Items.CostPrice ?? "0"),
                        (x.QuantityIn - x.QuantityOut) * Convert.ToDecimal(x.Items.CostPrice ?? "0"),
                        referenceNumber,
                        x.Items.BatchNumbers.Select(b => new BatchNumber
                        {
                            BatchNumbers = b.BatchNumbers,
                           
                        }).ToList(),
                       x.Items.ItemInstances != null
                            ? x.Items.ItemInstances.Select(ii => ii.SerialNumber).ToList()
                            : new List<string>(),
                       x.Items.ManufacturingAndExpiries != null
                            ? x.Items.ManufacturingAndExpiries.Select(me => new ManufactureAndExpiry
                            {
                                ExpiredDate=me.ExpiredDate
                                
                               
                            }).ToList()
                            : new List<ManufactureAndExpiry>()
                    ))
                    .ToListAsync();


                    PagedResult<GetStockReportQueryResponse> finalResponse;

                    if (paginationRequest.IsPagination)
                    {
                        int pageIndex = paginationRequest.pageIndex <= 0 ? 1 : paginationRequest.pageIndex;
                        int pageSize = paginationRequest.pageSize <= 0 ? 10 : paginationRequest.pageSize;

                        int totalItems = stockList.Count();

                        var pagedItems = stockList
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();

                        finalResponse = new PagedResult<GetStockReportQueryResponse>
                        {
                            Items = pagedItems,
                            TotalItems = totalItems,
                            PageIndex = pageIndex,
                            pageSize = pageSize
                        };
                    }
                    else
                    {
                        finalResponse = new PagedResult<GetStockReportQueryResponse>
                        {
                            Items = stockList,
                            TotalItems = stockList.Count(),
                            PageIndex = 1,
                            pageSize = stockList.Count()
                        };
                    }

                    return Result<PagedResult<GetStockReportQueryResponse>>.Success(finalResponse);
                
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the stock report.", ex);
            }
        }
    }
    
}
