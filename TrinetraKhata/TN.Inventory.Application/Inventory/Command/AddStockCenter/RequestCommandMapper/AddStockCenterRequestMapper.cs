using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockCenter.RequestCommandMapper
{
    public static class AddStockCenterRequestMapper
    {
        public static AddStockCenterCommand ToCommand(this AddStockCenterRequest request)
        {

            return new AddStockCenterCommand
                (
                    request.Name,
                    request.Address


                );
        }
    }
}
