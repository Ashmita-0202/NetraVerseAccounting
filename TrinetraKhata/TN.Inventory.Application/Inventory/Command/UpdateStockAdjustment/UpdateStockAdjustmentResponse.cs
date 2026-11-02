using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Inventory.StockAdjustment;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment
{
    public record  UpdateStockAdjustmentResponse
    (
             string id,
            string itemId,
            double quantityChanged,
            ReasonType reason,
            DateTime adjustedAt,
            string adjustedBy

    );
}
