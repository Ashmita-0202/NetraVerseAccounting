using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockTransferItems
{
    public record AddStockTransferItemsRequest
    (
             decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount
        );
    
}
