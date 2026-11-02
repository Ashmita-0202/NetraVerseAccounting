using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddStockCenter
{
    public record AddStockCenterResponse
    (

            string id,
            string Name,
            string? Address
    );
}
