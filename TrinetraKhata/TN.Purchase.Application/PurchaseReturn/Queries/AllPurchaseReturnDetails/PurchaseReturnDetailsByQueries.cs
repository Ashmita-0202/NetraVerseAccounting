

using MediatR;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails
{
    public record PurchaseReturnDetailsByQueries
    (PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<PurchaseReturnDetailsQueryResponse>>>;
}
