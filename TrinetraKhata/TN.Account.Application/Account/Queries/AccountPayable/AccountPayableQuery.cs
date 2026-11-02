using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Account.Application.Account.Queries.AccountPayable
{
    public record AccountPayableQuery(PaginationRequest PaginationRequest, string? ledgerId)
    : IRequest<Result<ARAPWithTotals>>;
    
}
