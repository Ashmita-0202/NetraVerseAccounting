using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.Profit_LossReport;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IProfitAndLossServices
    {
        Task<Result<PagedResult<ProfitAndLossFinalResponse>>> GetProfitLossReport(PaginationRequest PaginationRequest, string? CompanyId, CancellationToken cancellationToken = default);
    }
}
