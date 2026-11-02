using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.Annex13.Queries;
using TN.Reports.Application.BalanceSheet.Queries;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface IAnnexReportServices
    {
        Task<Result<PagedResult<AnnexReportQueryResponse>>> GetAnnexReport(PaginationRequest paginationRequest, AnnexReportDTOs annexReportDTOs, CancellationToken cancellationToken = default);
    }
}
