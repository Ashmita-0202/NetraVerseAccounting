using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails.RequestCommandMapper
{
    public static class AddPurchaseDetailsRequestMapper
    {
        public static AddPurchaseDetailsCommand ToCommand(this AddPurchaseDetailsRequest request)
        {
            return new AddPurchaseDetailsCommand
            (
                request.date,
                request.billNumber,
                request.ledgerId,
                request.amountInWords,
                request.discountPercent,
                request.discountAmount,
                request.vatPercent,
                request.vatAmount,
                request.grandTotalAmount,
                request.paymentId,
                request.referenceNumber,
                request.isPurchase,
                request.stockCenterId,
                request.chequeNumber,
                request.bankName,
                request.accountName,
                request.purchaseQuotationNumber,
                request.SubTotalAmount,
                request.TaxableAmount,
                request.AmountAfterVat,
                 request.BillSundryIds,
                request.PurchaseItems
                



            );
        }

    }
}