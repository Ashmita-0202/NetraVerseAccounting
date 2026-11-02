using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.AddUnits
{
    public class AddUnitsCommandValidator:AbstractValidator<AddUnitsCommand>
    {
        public AddUnitsCommandValidator() 
        {
            RuleFor(x => x.name)
                  .NotEmpty().WithMessage("units name is required.");
        }
    }
}
