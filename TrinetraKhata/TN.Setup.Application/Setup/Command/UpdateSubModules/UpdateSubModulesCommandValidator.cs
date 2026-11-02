using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateSubModules
{
    public  class UpdateSubModulesCommandValidator : AbstractValidator<UpdateSubModulesCommand>
    {
        public UpdateSubModulesCommandValidator()
        {
            RuleFor(x => x.name)
              .NotEmpty()
              .Matches(@"\S")
              .WithMessage("Submodules name is required");

            RuleFor(x => x.targetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");
        }
    }
}
