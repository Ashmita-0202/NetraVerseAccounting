using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Reports.Application.ItemRateHistory
{
    public record  GetItemRateHistoryQueryResponse
    (
          DateTime date,
          InventoriesType type,
          string? billNumber,
          string ledgerId,
          string itemId,
          string? itemGroupId,
          string? unitId,
          decimal quantity,
          decimal price,
          decimal amount
    );
}
