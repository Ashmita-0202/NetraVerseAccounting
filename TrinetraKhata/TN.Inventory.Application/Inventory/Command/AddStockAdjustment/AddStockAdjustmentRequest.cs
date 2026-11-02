using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Shared.Domain.Entities.Inventory.StockAdjustment;

namespace TN.Inventory.Application.Inventory.Command.AddStockAdjustment
{
    public record  AddStockAdjustmentRequest
    (
       
              string itemId,
            double quantityChanged,
            ReasonType reason
          

    );
}
