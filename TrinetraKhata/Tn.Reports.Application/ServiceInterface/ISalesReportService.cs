using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.DayBook.FilterSalesDayBook;
using TN.Reports.Application.DayBook.FilterSalesReturnDayBook;
using TN.Reports.Application.ItemwiseSalesReport;
using TN.Reports.Application.SalesReport;
using TN.Reports.Application.SalesReturn_Report;
using TN.Reports.Application.SalesSummaryReport;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface ISalesReportService
    {
        Task<PagedResult<GetSalesReportQueryResponse>> GetAllSalesReport(PaginationRequest paginationRequest, SalesReportDtos salesReportDtos);

        Task<Result<PagedResult<FilterSalesDayBookQueryResponse>>> GetFilterSalesDayBook(PaginationRequest paginationRequest, FilterSalesDayBookDto filterSalesDayBookDto);

        Task<Result<PagedResult<FilterSalesReturnDayBookQueryResponse>>> GetFilterSalesReturnDayBook(
            PaginationRequest paginationRequest, FilterSalesReturnDayBookDto filterSalesReturnDayBookDto, CancellationToken cancellationToken = default);
   
    Task<Result<PagedResult<GetSalesReturnReportQueryResponse>>> GetSalesReturnReport(PaginationRequest paginationRequest, SalesReturnReportDto salesReturnReportDto, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<GetSalesSummaryQueryResponse>>> GetSalesSummaryReport(PaginationRequest paginationRequest, SalesSummaryDtos salesSummaryDtos, CancellationToken cancellationToken = default);

        Task<Result<PagedResult<ItemwiseSalesReportQueryResponse>>> GetItemwiseSalesReport(PaginationRequest paginationRequest, ItemwiseSalesReportDto itemwiseSalesReportDto, CancellationToken cancellationToken = default);

    }
}
