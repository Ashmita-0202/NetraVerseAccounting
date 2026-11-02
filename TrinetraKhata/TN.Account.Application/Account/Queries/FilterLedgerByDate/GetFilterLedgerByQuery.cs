using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.FilterLedgerByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterLedger
{
    public record GetFilterLedgerByQuery
    (
        PaginationRequest PaginationRequest,
        FilterLedgerDto FilterLedgerDto
    ):IRequest<Result<PagedResult<GetFilterLedgerByResponse>>>;
}
