using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnItems
{
    public record AddPurchaseReturnItemsRequest
    (
            string purchaseItemsId="",
            decimal returnQuantity=0,
            decimal returnUnitPrice = 0,
            decimal returnTotalAmount = 0,
            string itemsId = ""
        );
}
