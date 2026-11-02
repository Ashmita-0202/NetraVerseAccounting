using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType.RequestCommandMapper
{
    public static class UpdatePurchaseReturnTypeRequestMapper
    {
        public static UpdatePurchaseReturnTypeCommand ToCommand(this UpdatePurchaseReturnTypeRequest request,string companyId)
        {
            return new UpdatePurchaseReturnTypeCommand(
              companyId,
                request.purchaseReturnNumberType
            );
        }
    }
}
