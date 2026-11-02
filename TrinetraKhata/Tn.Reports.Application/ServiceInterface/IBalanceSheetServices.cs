using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IBalanceSheetServices
    {
        Task<Result<PagedResult<BalanceSheetFinalResponse>>> GetBalanceSheetReport(PaginationRequest paginationRequest,string? companyId, CancellationToken cancellationToken = default);
    }
}
