using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems.RequestCommandMapper
{
    public static class AddPurchaseItemsRequestMapper
    {
        public static AddPurchaseItemsCommand ToCommand(this AddPurchaseItemsRequest request)
        {
            return new AddPurchaseItemsCommand
                (
                    request.quantity,
                    request.unitId,
                    request.itemId,
                    request.price,
                    request.amount

                    //request.purchaseDetailsId
                );
        }
    }
}