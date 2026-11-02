using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.UpdateConversionFactor
{
   public record UpdateConversionFactorResponse
    (
            string id,
            string fromUnit,
            string toUnit,
            decimal conversionFactor,
            DateTime createdAt,
            string userId,
            string updateBy,
            DateTime updatedAt
   );
}
