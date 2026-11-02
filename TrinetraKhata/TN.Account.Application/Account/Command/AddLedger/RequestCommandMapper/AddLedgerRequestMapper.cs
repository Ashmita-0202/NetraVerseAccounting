using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddLedgerGroup;

namespace TN.Account.Application.Account.Command.AddLedger.RequestCommandMapper
{
    public static class AddLedgerRequestMapper
    {
        public static AddLedgerCommand ToCommand(this AddLedgerRequest request)
        {
            return new AddLedgerCommand(request.name, request.isInventoryAffected,request.address,request.panNo,request.phoneNumber, request.maxCreditPeriod, request.maxDuePeriod, request.subledgerGroupId,request.openingBalance);
        }

    }
}
