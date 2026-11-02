using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;

namespace TN.Account.Application.Account.Command.AddLedgerGroup.RequestCommandMapper
{
    public static class AddLedgerRequestMapper
    {
       public static AddLedgerGroupCommand ToCommand(this AddLedgerGroupRequest request)
        {
            return new AddLedgerGroupCommand(request.name, request.isCustom,request.masterId, request.isPrimary);

        }

    }
}
