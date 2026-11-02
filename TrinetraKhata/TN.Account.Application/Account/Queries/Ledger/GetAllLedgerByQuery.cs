using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.LedgerGroup;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.Ledger
{
   public record GetAllLedgerByQuery(PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<GetAllLedgerByQueryResponse>>>;
}
