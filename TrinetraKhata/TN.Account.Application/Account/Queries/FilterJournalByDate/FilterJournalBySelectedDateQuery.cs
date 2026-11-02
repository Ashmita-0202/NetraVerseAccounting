using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.FilterJournalByDate
{
    public record  FilterJournalBySelectedDateQuery
    (
        PaginationRequest PaginationRequest,
        FilterJournalDTOs FilterJournalDTOs
    ) : IRequest<Result<PagedResult<FilterJournalBySelectedDateQueryResponse>>>;

}
