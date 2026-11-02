using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate
{
    public record FilterInventoryWithTotals
   (
    PagedResult<FilterInventoryByDateQueryResponse> PagedItems,
    decimal GrandTotalValue,
    decimal GrandTotalQuantity
);
}
