using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using static TN.Inventory.Domain.Entities.Inventories;

namespace TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate
{
    public record  FilterInventoryByDateQueryResponse
    (
              string id,
            string itemId,
            string itemNames,
            decimal remainingQuantity,
            decimal averageAmountIn,
            decimal averageAmountOut,
            DateTime entryDate,
            decimal salesQuantity,
            List<string> priceList,
            decimal totalValue

    );
}
