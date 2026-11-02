using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockAdjustment.RequestCommandMapper
{
    public static class AddStockAdjustmentRequestMapper
    {
        public static AddStockAdjustmentCommand ToCommand(this AddStockAdjustmentRequest request)
        {
            return new AddStockAdjustmentCommand
            (
                request.itemId,
                request.quantityChanged,
                request.reason
              
            );
        }
    }
}
