using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook
{
    public record  PurchaseReturnDayBookQuery
    (
        PaginationRequest PaginationRequest,
        PurchaseReturnDayBookDto PurchaseReturnDayBookDto
    ) :IRequest<Result<PagedResult<PurchaseReturnDayBookQueryResponse>>>;
}
