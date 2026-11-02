using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Queries.StockCentersById
{
    public record  GetStockQueryByIdResponse
    (

          string Name,
            string? Address
    );
}
