using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Inventory.Application.Inventory.Command.UpdateUnits
{
    public class UpdateUnitsCommandValidator:AbstractValidator<UpdateUnitsCommand>
    {
        public UpdateUnitsCommandValidator() 
        {
            RuleFor(x => x.name)
                 .NotEmpty().WithMessage("units name is required.");
        }
    }
}
