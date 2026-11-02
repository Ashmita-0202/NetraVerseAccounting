using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Reports.Application.ItemRateHistory
{
    public record ItemRateHistoryDtos
    (
        string? startDate,
        string? endDate,
        string? stockCenterId,
        string? itemId,
        string? itemGroupId,
        string? ledgerId,
       InventoriesType? type

    );
}
