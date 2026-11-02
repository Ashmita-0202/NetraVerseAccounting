using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate
{
    public record FilterInventoryDtos
     (
        string? itemId,
        string? startDate,
        string? endDate
    );
}
