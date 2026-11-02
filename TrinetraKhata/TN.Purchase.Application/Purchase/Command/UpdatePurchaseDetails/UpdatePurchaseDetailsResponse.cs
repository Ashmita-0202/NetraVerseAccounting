using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails
{
    public record UpdatePurchaseDetailsResponse
   (
             string id,
             string billNumber,
             string ledgerId,
             string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
             decimal grandTotalAmount,
             string paymentId,
             string? referenceNumber,
             string? StockCenterId,
            List<UpdatePurchaseItems> PurchaseItems
    );
    public record UpdatePurchaseItems
        (
           decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount,
           string purchaseDetailsId
        );

}
