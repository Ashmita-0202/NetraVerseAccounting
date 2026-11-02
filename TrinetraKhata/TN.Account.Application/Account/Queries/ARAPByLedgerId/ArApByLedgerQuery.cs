using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.ARAPByLedgerId
{
    public record ArApByLedgerQuery
   (
        string ledgerId
        ) : IRequest<Result<ArApByLedgerQueryResponse>>;
}
