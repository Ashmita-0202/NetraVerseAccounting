using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignMenusToRole
{
    public class AssignMenuToRolesValidator: AbstractValidator<AssignMenusToRoleCommands>
    {
        public AssignMenuToRolesValidator()
        {
            RuleFor(x => x.roleId).
               NotEmpty()
               .Matches(@"\S")
               .WithMessage("RoleId is required");
        }
    }
}
