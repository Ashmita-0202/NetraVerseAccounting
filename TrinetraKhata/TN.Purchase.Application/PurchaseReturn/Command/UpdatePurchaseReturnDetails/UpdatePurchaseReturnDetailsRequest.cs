using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails
{
  public record UpdatePurchaseReturnDetailsRequest
   (
            
            string purchaseDetailsId,
            DateTime returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal netReturnAmount,
            string companyId,
            List<UpdatePurchaseReturnItemsDTOs> purchaseReturnItems
  );
    public record UpdatePurchaseReturnItemsDTOs
        (
            string id,
            string purchaseReturnDetailsId,
            string purchaseItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalPrice

        );
}
