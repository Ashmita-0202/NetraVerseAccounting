using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.QuotationToPurchase.RequestCommandMapper
{
    public static class QuotationToPurchaseRequestMapper
    {
        public static QuotationToPurchaseCommand ToCommand(this QuotationToPurchaseRequest request)
        {
            return new QuotationToPurchaseCommand
                (
                request.purchaseQuotationId,
                request.paymentId,
                request.billNumbers,
                request.chequeNumber,
                request.bankName,
                request.accountName


                );
        }
    }
}
