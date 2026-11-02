using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.master;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.LedgerGroup
{
    public record GetAllLedgerGroupByQuery(PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<GetAllLedgerGroupByQueryResponse>>>;
}
