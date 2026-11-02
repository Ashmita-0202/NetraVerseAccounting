using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerBalanceReport
{
    public record LedgerBalanceReportQueries
    (
        PaginationRequest PaginationRequest, LedgerBalanceDTOs ledgerBalanceDTOs 
        ) : IRequest<Result<PagedResult<LedgerBalanceReportQueryResponse>>>;
}
