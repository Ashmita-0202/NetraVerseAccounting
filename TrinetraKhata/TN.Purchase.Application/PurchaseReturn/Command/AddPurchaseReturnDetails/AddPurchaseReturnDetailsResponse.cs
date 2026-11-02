using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnItems;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails
{
    public record AddPurchaseReturnDetailsResponse
   (
        string id="",
            string purchaseDetailsId = "",
            DateTime returnDate = default,
            decimal totalReturnAmount=0,
            decimal taxAdjustment = 0,
            decimal netReturnAmount=0,
            string companyId = "",
            string reason = "",
            List<AddPurchaseReturnItemsRequest>? PurchaseReturnItems = null
        );
}
