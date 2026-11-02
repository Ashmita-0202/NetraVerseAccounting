using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.AddConversionFactor
{
   public class AddConversionFactorCommandValidator:AbstractValidator<AddConversionFactorCommand>
    {
        public AddConversionFactorCommandValidator() 
        {
            RuleFor(x => x.fromUnit)
                .NotEmpty().WithMessage("From unit is required.");


        }
    }
}
