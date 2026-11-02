using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.StockDetailReport.Queries
{
    public record  GetStockDetailReportQuery
  (string companyId,PaginationRequest PaginationRequest,FilterStockDetailReportDto FilterStockDetailReportDto):IRequest<Result<PagedResult<GetStockDetailReportResponse>>>;
}
