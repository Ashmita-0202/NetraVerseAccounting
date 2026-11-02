using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.StockDetailReport.Queries;
using TN.Reports.Application.StockReport.Queries;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IStockDetailReportService
    {
        Task<Result<PagedResult<GetStockDetailReportResponse>>> GetStockDetailReport(string companyId,PaginationRequest paginationRequest,FilterStockDetailReportDto filterStockDetailReportDto,CancellationToken cancellationToken=default);

        Task<Result<PagedResult<GetStockReportQueryResponse>>> GetStockReport(PaginationRequest paginationRequest, StockReportDtos stockReportDto, CancellationToken cancellationToken = default);

    }
}
