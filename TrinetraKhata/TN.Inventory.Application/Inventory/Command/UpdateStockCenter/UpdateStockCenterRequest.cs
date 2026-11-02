using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockCenter
{
    public record UpdateStockCenterRequest
    (

          string Name,
            string? Address

    );
}
