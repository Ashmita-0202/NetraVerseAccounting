using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Inventory.StockAdjustment;

namespace TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment
{
    public record  FilterStockAdjustmentQueryResponse
    (
            string id,
            string itemId,
            double quantityChanged,
            ReasonType reason,
            DateTime adjustedAt,
            string adjustedBy

    );
}
