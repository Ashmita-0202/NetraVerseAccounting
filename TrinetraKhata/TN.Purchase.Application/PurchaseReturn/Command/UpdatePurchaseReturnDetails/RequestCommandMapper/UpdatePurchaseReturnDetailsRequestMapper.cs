using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails.RequestCommandMapper
{
    public static class UpdatePurchaseReturnDetailsRequestMapper
    {
        public static  UpdatePurchaseReturnDetailsCommand ToCommand(this UpdatePurchaseReturnDetailsRequest request, string id)
        {
            return new UpdatePurchaseReturnDetailsCommand
                (
                  id,
                  request.purchaseDetailsId,
                  request.returnDate,
                  request.totalReturnAmount,
                  request.taxAdjustment,
                  request.netReturnAmount,
                  request.companyId,
                  request.purchaseReturnItems

                );
        }
    }
}
