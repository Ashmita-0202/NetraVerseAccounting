using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Purchase.Domain.Entities.PurchaseDetails;

namespace TN.Purchase.Application.Purchase.Queries.Purchase
{
    public record GetAllPurchaseDetailsQueryResponse
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
             PurchaseStatus status,
             string referenceNumber,
             string paymentId,
             string stockCenterId,
            List<PurchaseItemsDto> PurchaseItems
    );

    public record PurchaseItemsDto
     (
            string id,
            decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt,
            string hsCode,
            bool? isVatEnabled,
            List<string?> serialNumbers
     );
    public record QuantityDetailDto
   (
       string purchaseItemId,
       string itemId,
       string unitId,
       decimal quantity,
        List<string?> serialNumbers

   );
}
