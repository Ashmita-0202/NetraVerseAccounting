using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.UpdateConversionFactor
{
    public class UpdateConversionFactorCommandValidator:AbstractValidator<UpdateConversionFactorCommand>
    {
        public UpdateConversionFactorCommandValidator()
        {
            RuleFor(x => x.fromUnit)
               .NotEmpty().WithMessage("From unit is required.");
        }
    }
}
