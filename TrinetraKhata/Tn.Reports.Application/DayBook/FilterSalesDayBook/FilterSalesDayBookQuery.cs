using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.FilterSalesDayBook
{
    public record FilterSalesDayBookQuery
    (
        PaginationRequest PaginationRequest,
       FilterSalesDayBookDto FilterSalesDayBookDto

    ) : IRequest<Result<PagedResult<FilterSalesDayBookQueryResponse>>>;
}
