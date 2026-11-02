using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignSubModules
{
    public class UpdateAssignSubModulesCommandValidator: AbstractValidator<UpdateAssignSubModulesCommand>
    {
        public UpdateAssignSubModulesCommandValidator()
        {
            RuleFor(x => x.subModulesId)
                        .NotEmpty()
                        .WithMessage("ModulesId is required.");


            RuleFor(x => x.roleId)
                .NotEmpty()
                .WithMessage("RolesId is required.");
        }
    }
}
