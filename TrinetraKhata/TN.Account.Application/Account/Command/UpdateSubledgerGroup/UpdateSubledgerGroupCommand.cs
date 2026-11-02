using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup
{
    public record  UpdateSubledgerGroupCommand
    (       
            string id,
            string name,
            string ledgerGroupId
   ):IRequest<Result<UpdateSubledgerGroupResponse>>;
}
