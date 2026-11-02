using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddUnits.RequestCommandMapper
{
   public static class AddUnitsRequestMapper
    {
        public static AddUnitsCommand ToCommand(this AddUnitsRequest request)
        {
            return new AddUnitsCommand
                
                (
                    request.name,
                    request.isEnabled
                
                );
        
        }
    }
}
