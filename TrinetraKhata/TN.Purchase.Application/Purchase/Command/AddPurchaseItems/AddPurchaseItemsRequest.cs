using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public record AddPurchaseItemsRequest
    (
            decimal quantity=0,
            string unitId="",
            string itemId="",
            decimal price = 0,
            decimal amount = 0,
                List<ExpiryAndManufactureDTOs>? manufactureAndExpiries=null,
                List<BatchNumberDTOs>? batchNumbers=null,
            List<string>? serialNumbers= null
    );
}