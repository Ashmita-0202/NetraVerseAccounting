using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddSubledgerGroup.RequestCommandMapper
{
    public static class AddSubledgerGroupRequestMapper
    {
        public static AddSubledgerGroupCommand ToCommand(this AddSubledgerGroupRequest request)
        {
            return new AddSubledgerGroupCommand
            (
               request.name,
                 request.ledgerGroupId
            );
        }
    }
}
