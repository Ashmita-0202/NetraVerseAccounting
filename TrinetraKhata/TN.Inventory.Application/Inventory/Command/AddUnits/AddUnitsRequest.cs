using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddUnits
{
    public record AddUnitsRequest
    (
        
            string name,
            bool isEnabled
        );
}
