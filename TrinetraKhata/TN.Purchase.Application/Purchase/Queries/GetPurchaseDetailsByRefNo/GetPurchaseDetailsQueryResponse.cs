using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo
{
    public record  GetPurchaseDetailsQueryResponse
    (
            string id,
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            decimal discountPercent,
            decimal discountAmount,
            decimal vatPercent,
            decimal vatAmount,
            string companyId,
           decimal grandTotalAmount,
            string referenceNumber,
            string paymentId,
            string stockCenterId,
           List<PurchaseItemsDto> PurchaseItems
    );
}
