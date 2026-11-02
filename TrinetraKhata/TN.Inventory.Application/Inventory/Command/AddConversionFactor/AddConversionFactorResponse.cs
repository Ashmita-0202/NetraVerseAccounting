using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor
{
    public record class AddConversionFactorResponse(
        string name="",
      string FromUnit = "",
      string ToUnit = "",
      decimal ConversionFactor=0,
      DateTime CreatedAt=default,
      string UserId = "",
      string UpdateBy="",
      DateTime UpdatedAt=default
  );

}
