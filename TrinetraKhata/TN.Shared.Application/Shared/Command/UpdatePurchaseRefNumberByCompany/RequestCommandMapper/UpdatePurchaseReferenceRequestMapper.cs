using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany.RequestCommandMapper
{
    public static class UpdatePurchaseReferenceRequestMapper
    {
        public static UpdatePurchaseReferenceNumberCommand ToCommand(this UpdatePurchaseReferenceNumberRequest request, string companyId) 
        {
            return new UpdatePurchaseReferenceNumberCommand
                (
                    request.purchaseReference,
                    companyId
                
                );
        
        }
    }
}
