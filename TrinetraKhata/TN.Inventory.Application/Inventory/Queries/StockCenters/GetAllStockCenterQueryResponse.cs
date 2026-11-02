using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.StockCenters
{
    public record GetAllStockCenterQueryResponse
    (
           string Id,
            string Name,
            string? Address

    );
}
