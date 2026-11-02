using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.LedgerByLedgerGroupId
{
    public record GetAllLedgerByLedgerGroupIdQuery
    (
        string ledgerGroupId
        ): IRequest<Result<List<GetAllLedgerByLedgerGroupIdResponse>>>;
}
