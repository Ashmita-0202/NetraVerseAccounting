using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddLedgerGroup
{
    public record AddLedgerGroupCommand
    (
        string name,
            bool isCustom,
            string masterId,
            bool isPrimary

       ) : IRequest<Result<AddLedgerGroupResponse>>;
    
}
