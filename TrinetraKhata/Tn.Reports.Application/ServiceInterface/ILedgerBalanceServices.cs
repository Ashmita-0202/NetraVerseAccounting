using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport;
using TN.Reports.Application.LedgerBalance.Queries.LedgerSummary;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.ServiceInterface
{
    public interface ILedgerBalanceServices
    {
        Task<Result<PagedResult<LedgerBalanceReportQueryResponse>>> GetLedgerBalanceReportByLedger(PaginationRequest paginationRequest, LedgerBalanceDTOs ledgerBalanceDTOs);

        Task<Result<PagedResult<LedgerSummaryResponse>>> GetLedgerSummaryByLedger(PaginationRequest paginationRequest, string ledgerId);
    }
}
