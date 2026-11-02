using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.UpdateLedgerGroup;

namespace TN.Account.Application.Account.Command.UpdateLedger.RequestCommandMapper
{
    public static class UpdateLedgerRequestMapper
    {
        public static UpdateLedgerCommand ToCommand(this UpdateLedgerRequest request, string ledgerId)
        {
            return new UpdateLedgerCommand(ledgerId, request.name, request.isInventoryAffected,request.address,request.panNo,request.phoneNumber, request.maxCreditPeriod, request.maxDuePeriod,request.subledgerGroupId);
        }
    }
}


