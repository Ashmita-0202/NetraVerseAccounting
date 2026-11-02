using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermission
{
    public class AddPermissionCommandValidator: AbstractValidator<AddPermissionCommand>
    {
        public AddPermissionCommandValidator()
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
