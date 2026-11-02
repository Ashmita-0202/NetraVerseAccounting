using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Reports.Application.GodownwiseInventoryReport
{
    public record  GetGodownwiseInventoryQueryResponse
    (
           string stockCenterId,
           string itemId,
           string? itemGroupId,
           decimal inwardQty,
           decimal outwardQty,
           decimal openingStockQuantity,
           string adjustmentId,
           decimal closingStockQuantity,
           decimal value


    );
}
