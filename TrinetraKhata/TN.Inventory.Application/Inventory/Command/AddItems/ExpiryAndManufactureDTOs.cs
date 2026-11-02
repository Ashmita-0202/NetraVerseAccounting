using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddItems
{
    public record ExpiryAndManufactureDTOs
    (
        DateTime? expiryDate,
        DateTime? manufactureDate,
        decimal? totalQuantity

        );
    
}
