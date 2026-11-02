using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles
{
    public class AddPermissionToRolesCommandValidator : AbstractValidator<AddPermissionToRolesCommand>
    {
        public AddPermissionToRolesCommandValidator()
        {
            RuleFor(x => x.permissionId)
                         .NotEmpty()
                         .WithMessage("permissionIds is required.");


        }
    }
}
