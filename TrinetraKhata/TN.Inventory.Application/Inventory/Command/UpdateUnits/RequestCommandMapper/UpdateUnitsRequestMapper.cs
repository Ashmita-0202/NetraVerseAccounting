using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits.RequestCommandMapper
{
  public static class UpdateUnitsRequestMapper
    {
       public static UpdateUnitsCommand ToCommand(this UpdateUnitsRequest request, string id)
        {
            return new UpdateUnitsCommand
                (
                    id,
                    request.name,
                    request.isEnabled
                );
        }
    }
}
