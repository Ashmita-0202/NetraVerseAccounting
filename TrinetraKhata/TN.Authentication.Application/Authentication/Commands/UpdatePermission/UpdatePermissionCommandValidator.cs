using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Authentication.Application.Authentication.Commands.UpdatePermission
{
    public class UpdatePermissionCommandValidator:AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
        {
            RuleFor(x => x.name)
                         .NotEmpty()
                         .WithMessage("Permission name is required.");
            RuleFor(x => x.roleId)
                         .NotEmpty()
                         .WithMessage("Permission name is required.");
        }
    }
}
