using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems
{
    public record  GetAllPurchaseItemsByQueryResponse
    (
            string id,
            decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            string amount,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt,
            string purchaseDetailsId

    );
    
}
