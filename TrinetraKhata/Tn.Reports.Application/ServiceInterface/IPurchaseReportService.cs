using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IPurchaseReportService
    {
        Task<PagedResult<GetPurchaseReportQueryResponse>> GetAllPurchaseReport(PaginationRequest paginationRequest, PurchaseReportDtos purchaseReportDtos);
        Task<Result<PagedResult<FilterPurchaseDayBookQueryResponse>>> GetFilterPurchaseDayBook(
            PaginationRequest paginationRequest, FilterPurchaseDayBookDto filterPurchaseDayBookDto, CancellationToken cancellationToken = default);
  
    Task<Result<PagedResult<PurchaseReturnDayBookQueryResponse>>> GetPurchaseReturnDayBook(
        PaginationRequest paginationRequest, PurchaseReturnDayBookDto purchaseReturnDayBookDto, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<CashDayBookQueryResponse>>> GetCashDayBook(PaginationRequest paginationRequest, CashDayBookDto cashDayBookDto, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetPurchaseReturnReportQueryResponse>>> GetPurchaseReturnReport(PaginationRequest paginationRequest, PurchaseReturnReportDto purchaseReturnReportDto, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetItemRateHistoryQueryResponse>>> GetItemRateHistory(
            PaginationRequest paginationRequest, ItemRateHistoryDtos itemRateHistoryDtos, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetItemwisePurchaseExpireDateResponse>>> GetItemwisePurchaseByExpireDate(PaginationRequest paginationRequest, ItemwisePurchaseExpireDateDtos itemwisePurchaseExpireDateDtos, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetPurchaseSummaryResponse>>> PurchaseSummaryReport(PaginationRequest paginationRequest, PurchaseSummaryDtos purchaseSummaryDtos, CancellationToken cancellationToken = default);
 
        Task<Result<PagedResult<GetGodownwiseInventoryQueryResponse>>> GetGodownwiseInventory(PaginationRequest paginationRequest,GodownwiseInventoryDtos godownwiseInventoryDtos, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetItemwiseProfitQueryResponse>>> GetItemwiseProfit(PaginationRequest paginationRequest, ItemwiseProfitDtos itemwiseProfitDtos,CancellationToken cancellationToken = default);

         Task<Result<PagedResult<ItemwisePurchaseReportQueryResponse>>> GetItemwisePurchaseReport(PaginationRequest paginationRequest, ItemwisePurchaseReportDto itemwisePurchaseReportDto, CancellationToken cancellationToken = default);

    }
}
