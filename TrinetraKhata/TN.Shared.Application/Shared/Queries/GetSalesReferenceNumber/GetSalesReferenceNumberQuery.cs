using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetSalesReferenceNumber
{
    public record GetSalesReferenceNumberQuery
    (
        string companyId
        ) : IRequest<Result<GetSalesReferenceNumberQueryResponse>>;
}
