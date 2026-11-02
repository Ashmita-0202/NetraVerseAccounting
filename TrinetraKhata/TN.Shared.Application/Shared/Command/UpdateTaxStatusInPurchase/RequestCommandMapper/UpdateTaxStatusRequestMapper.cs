using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase.RequestCommandMapper
{
    public static class UpdateTaxStatusRequestMapper
    {
        public static UpdateTaxStatusInPurchaseCommand ToCommand(this UpdateTaxStatusInPurchaseRequest request, string companyId) 
        {
            return new UpdateTaxStatusInPurchaseCommand
                (
                     companyId,
                    request.showTaxInPurchase
                    
                );
        
        }
    }
}
