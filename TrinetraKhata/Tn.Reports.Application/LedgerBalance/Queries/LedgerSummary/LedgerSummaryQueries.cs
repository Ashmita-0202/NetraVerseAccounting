using Azure.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerSummary
{
    public record LedgerSummaryQueries
    (
        PaginationRequest PaginationRequest,
        string ledgerId
        ) : IRequest<Result<PagedResult<LedgerSummaryResponse>>>;
}
