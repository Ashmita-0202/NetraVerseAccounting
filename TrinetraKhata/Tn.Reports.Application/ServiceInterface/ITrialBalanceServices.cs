
using TN.Reports.Application.TrialBalance;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface ITrialBalanceServices
    {
        Task<Result<PagedResult<MasterLevelQueryRespones>>> GetTrialBalanceReport(PaginationRequest paginationRequest, string? companyId, CancellationToken cancellationToken = default);
    }
}
