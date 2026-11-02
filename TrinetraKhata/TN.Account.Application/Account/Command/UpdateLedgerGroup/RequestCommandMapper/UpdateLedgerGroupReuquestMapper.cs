using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateLedgerGroup.RequestCommandMapper
{
    public static class UpdateLedgerGroupReuquestMapper
    {
        public static UpdateLedgerGroupCommand ToCommand(this UpdateLedgerGroupRequest request, string ledgerGroupId)
        {
            return new UpdateLedgerGroupCommand(ledgerGroupId, request.name, request.isCustom, request.masterId, request.isPrimary);
        }
        
    }
}
