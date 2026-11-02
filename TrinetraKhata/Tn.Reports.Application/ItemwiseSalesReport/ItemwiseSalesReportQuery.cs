using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ItemwiseSalesReport
{
    public record  ItemwiseSalesReportQuery
    (PaginationRequest PaginationRequest,ItemwiseSalesReportDto ItemwiseSalesReportDto) :IRequest<Result<PagedResult<ItemwiseSalesReportQueryResponse>>>;
}
