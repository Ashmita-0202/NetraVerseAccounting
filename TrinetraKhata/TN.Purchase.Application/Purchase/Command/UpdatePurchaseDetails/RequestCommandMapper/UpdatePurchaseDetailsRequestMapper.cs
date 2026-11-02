using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails.RequestCommandMapper
{
   public static class UpdatePurchaseDetailsRequestMapper
    {
        public static UpdatePurchaseDetailsCommand ToCommand(this UpdatePurchaseDetailsRequest request, string id) 
        {
            return new UpdatePurchaseDetailsCommand
            (
                id,
                request.date,
                request.billNumber,
                request.ledgerId,
                request.amountInWords,
                request.discountPercent,
                request.discountAmount,
                request.vatPercent,
                request.vatAmount,
               request. grandTotalAmount,
               request.paymentId,
               request.referenceNumber,
                request.subTotalAmount,
                request.taxableAmount,
                request.amountAfterVat,
                 request.BillSundryIds,
                request.PurchaseItems
            );
        }
    }
}
