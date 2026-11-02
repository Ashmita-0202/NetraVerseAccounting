using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.FilterPurchaseDayBook
{
    public record FilterPurchaseDayBookQuery
    (

          PaginationRequest PaginationRequest,
            FilterPurchaseDayBookDto FilterPurchaseDayBookDto
    ) : IRequest<Result<PagedResult<FilterPurchaseDayBookQueryResponse>>>;

}
