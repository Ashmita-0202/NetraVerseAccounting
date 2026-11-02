using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.QuotationToPurchase
{
    public record QuotationToPurchaseRequest
    (
        string purchaseQuotationId,
        string? paymentId,
        string? billNumbers,
        string? chequeNumber,
        string? bankName,
        string? accountName

        );

}
