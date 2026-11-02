using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateModules
{
    public class UpdateModulesCommandValidators : AbstractValidator<UpdateModulesCommand>
    {
        public UpdateModulesCommandValidators()
        {
            RuleFor(x => x.Name).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Module name is required");

            RuleFor(x => x.Rank).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Role name is required");

            RuleFor(x => x.IconUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("IConUrl name is required");

            RuleFor(x => x.TargetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");


        }
    }
}
