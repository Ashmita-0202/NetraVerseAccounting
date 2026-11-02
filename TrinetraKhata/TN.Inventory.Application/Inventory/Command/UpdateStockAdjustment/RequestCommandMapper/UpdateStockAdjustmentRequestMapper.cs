using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment.RequestCommandMapper
{
    public static class UpdateStockAdjustmentRequestMapper
    {
        public static UpdateStockAdjustmentCommand ToCommand(this UpdateStockAdjustmentRequest request,string id)
        {
            return new UpdateStockAdjustmentCommand
            (
                 id,
                request.itemId,
                request.quantityChanged,
                request.reason
             

            );
        }
    }
}
