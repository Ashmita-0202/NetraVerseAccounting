using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor
{
   public record AddConversionFactorCommand
   (
            string name,
            string fromUnit,
            string toUnit,
            decimal conversionFactor
      

   ) :IRequest<Result<AddConversionFactorResponse>>;
}
