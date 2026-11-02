using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.AccountReceivable
{
    public record AccountReceivableQuery
    (PaginationRequest PaginationRequest, string? ledgerId
        ) : IRequest<Result<PagedResult<AccountReceivableQueryResponse>>>;      
}
