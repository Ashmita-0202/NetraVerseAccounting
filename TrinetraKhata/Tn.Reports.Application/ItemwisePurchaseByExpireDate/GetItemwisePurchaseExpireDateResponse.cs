using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Purchase.Domain.Entities.PurchaseDetails;

namespace TN.Reports.Application.ItemwisePurchaseByExpireDate
{
    public record  GetItemwisePurchaseExpireDateResponse
    (
              string itemId,
              string date,
              string? itemGroupId,
              string? expiredDate,
              string billNumber,
              string referenceNumber,
              string ledgerId,
              decimal quantity,
              decimal price,
              decimal amount,
              decimal discountAmount,
              decimal vatAmount,
              decimal grandTotalAmount,
              string stockCenterId
         

    );
}
