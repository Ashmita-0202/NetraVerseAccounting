using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.FilterStockCenter
{
    public record  FilterStockCenterDto
   (
        string? name,
        string? startDate,
        string? endDate

     );
}
