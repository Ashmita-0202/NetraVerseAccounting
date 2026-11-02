using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails.RequestCommandMapper
{
    public static class AddPurchaseReturnDetailsMapper
    {
        public static AddPurchaseReturnDetailsCommand ToCommand(this AddPurchaseReturnDetailsRequest request)
        {
            return new AddPurchaseReturnDetailsCommand
                (
                request.purchaseDetailsId,
                request.returnDate,
                request.totalReturnAmount,
                request.taxAdjustment,
                request.discount,
                request.netReturnAmount,
                request.reason,
                request.paymentId,
                request.purchaseReturnNumber,
                request.chequeNumber,
                request.bankName,
                request.accountName,
                request.subTotalAmount,
                request.taxableAmount,
                request.amountAfterVat,
                request.BillSundryIds,
                request.PurchaseReturnItems
                
                );
            
        }
    }
}
