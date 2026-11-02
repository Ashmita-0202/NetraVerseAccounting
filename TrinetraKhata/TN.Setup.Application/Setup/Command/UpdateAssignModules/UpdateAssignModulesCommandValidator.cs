using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignModules
{
    public class UpdateAssignModulesCommandValidator : AbstractValidator<UpdateAssignModulesCommand>
    {
        public UpdateAssignModulesCommandValidator()
        {
            RuleFor(x => x.modulesId)
                        .NotEmpty()
                        .WithMessage("ModulesId is required.");


            RuleFor(x => x.roleId)
                .NotEmpty()
                .WithMessage("RolesId is required.");
        }
    }
}
