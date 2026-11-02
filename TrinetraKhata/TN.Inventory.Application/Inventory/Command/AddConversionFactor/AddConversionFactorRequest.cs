using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor
{
  public record AddConversionFactorRequest
    (
      string name,
            string fromUnit,
            string toUnit,
            decimal conversionFactor
          
  );
}
