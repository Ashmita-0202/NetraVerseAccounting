using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById
{
    public record PurchaseReturnDetailsByIdQueries( string id): IRequest<Result<PurchaseReturnDetailsByIdQueryResponse>>;
}
