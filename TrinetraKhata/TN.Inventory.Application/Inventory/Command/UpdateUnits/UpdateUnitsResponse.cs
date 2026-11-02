using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits
{
   public record UpdateUnitsResponse
   (
             string id="",
            string name = "",
            DateTime createdAt= default,
            string userId = "",
            DateTime updatedAt = default,
            string updatedBy = "",
            bool isEnabled = true
       );
}
