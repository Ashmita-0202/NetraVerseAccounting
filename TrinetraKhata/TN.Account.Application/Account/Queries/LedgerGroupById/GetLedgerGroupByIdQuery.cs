using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.Account.Queries.GetMasterById;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.LedgerGroupById
{
    public record class GetLedgerGroupByIdQuery(string id): IRequest<Result<GetLedgerGroupByIdResponse>>;
}
