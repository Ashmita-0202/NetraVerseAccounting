using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockCenter.RequestCommandMapper
{
    public static class UpdateStockCenterRequestMapper
    {
        public static UpdateStockCenterCommand ToUpdateStockCenterCommand(this UpdateStockCenterRequest request, string id)
        {
            return new UpdateStockCenterCommand
            (
                id,
                request.Name,
                request.Address
                
            );
        }
    }
}
