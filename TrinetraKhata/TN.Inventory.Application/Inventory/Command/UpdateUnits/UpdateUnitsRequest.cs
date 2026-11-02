using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits
{
    public record UpdateUnitsRequest
    (
            string name,
            bool isEnabled
     );
}
