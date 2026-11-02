using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs
{
    public record  GetAllInventoriesLogsByQueryResponse
   (
            string id,
            string itemId,
            double quantityIn,
            double amountIn,
            DateTime entryDate,
            string ledgerId,
            string companyId,
            InventoriesType type,
            DateTime logDate,
            string userId
    );
}
