using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateConversionFactor
{
   public record UpdateConversionFactorRequest
    (
            string fromUnit,
            string toUnit,
            decimal conversionFactor,
            DateTime createdAt,
            string userId
   );
}
