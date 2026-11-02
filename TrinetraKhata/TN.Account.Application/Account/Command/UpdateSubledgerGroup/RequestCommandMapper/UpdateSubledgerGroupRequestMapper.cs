using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateSubledgerGroup.RequestCommandMapper
{
    public static class  UpdateSubledgerGroupRequestMapper
    {
        public static UpdateSubledgerGroupCommand ToCommand(this UpdateSubledgerGroupRequest request,string id)
        {
            return new UpdateSubledgerGroupCommand
            (
               id,
               request.name,
               request.ledgerGroupId
            );
        }
    }
}
