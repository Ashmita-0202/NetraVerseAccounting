

using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.OpeningClosingBalance
{
    public record OpeningClosingBalanceQuery
    (
        string fyId,
        PaginationRequest paginationRequest
        ) : IRequest<Result<PagedResult<OpeningClosingBalanceResponse>>>;
}
