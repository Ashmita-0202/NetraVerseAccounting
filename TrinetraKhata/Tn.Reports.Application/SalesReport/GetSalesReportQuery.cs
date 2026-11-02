using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.SalesReport
{
    public record  GetSalesReportQuery
    (PaginationRequest paginationRequest, SalesReportDtos salesReportDtos):IRequest<Result<PagedResult<GetSalesReportQueryResponse>>>;
    
}
