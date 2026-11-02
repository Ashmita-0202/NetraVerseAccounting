using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.UpdateCompany;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase.RequestCommandMapper
{
    public static class UpdateBillNumberStatusForPurchaseRequestMapper
    {
        public static UpdateBillNumberStatusForPurchaseCommand ToCommand(this UpdateBillNumberStatusForPurchaseRequest request, string id)
        { 
            return new UpdateBillNumberStatusForPurchaseCommand
                (
                     id,
                     request.billNumberGenerationTypeForPurchase
                );
        }
    }
}
