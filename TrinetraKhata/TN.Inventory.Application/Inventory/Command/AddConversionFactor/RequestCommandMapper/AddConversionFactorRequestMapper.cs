using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor.RequestCommandMapper
{
    public static class AddConversionFactorRequestMapper
    {
        public static AddConversionFactorCommand ToCommand(this AddConversionFactorRequest request)
        {
            return new AddConversionFactorCommand
                (
                request.name,
                    request.fromUnit,
                    request.toUnit,
                    request.conversionFactor
                   


                );
        }
    }
}
