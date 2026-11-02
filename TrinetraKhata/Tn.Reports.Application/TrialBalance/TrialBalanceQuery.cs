using MediatR;

using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.TrialBalance
{
    public record TrialBalanceQuery
        (PaginationRequest PaginationRequest, string? companyId) : IRequest<Result<PagedResult<MasterLevelQueryRespones>>>;

}
