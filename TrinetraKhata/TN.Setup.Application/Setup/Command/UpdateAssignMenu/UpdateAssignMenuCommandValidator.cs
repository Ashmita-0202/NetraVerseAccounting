using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignMenu
{
    public class UpdateAssignMenuCommandValidator: AbstractValidator<UpdateAssignMenuCommand>
    {
        public UpdateAssignMenuCommandValidator()
        {
            RuleFor(x => x.menuId)
                        .NotEmpty()
                        .WithMessage("ModulesId is required.");


            RuleFor(x => x.roleId)
                .NotEmpty()
                .WithMessage("RolesId is required.");
        }
    }
}
