using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate
{
    public record  FilterPurchaseDetailsByDateQuery
    (PaginationRequest PaginationRequest,FilterPurchaseDetailsDTOs FilterPurchaseDetailsDTOs):IRequest<Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>>;
}
