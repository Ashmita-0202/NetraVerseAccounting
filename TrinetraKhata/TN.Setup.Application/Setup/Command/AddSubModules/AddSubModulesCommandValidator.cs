using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddSubModules
{
    public class AddSubModulesCommandValidator: AbstractValidator<AddSubModulesCommand>
    {
        public AddSubModulesCommandValidator() 
        {
            RuleFor(x => x.name)
                .NotEmpty()
                .Matches(@"\S")
                .WithMessage("Submodules name is required");

            RuleFor(x => x.TargetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");
        }
    }
}
