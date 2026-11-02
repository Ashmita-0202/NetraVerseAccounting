using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AssignRoles
{
    public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
    {
        public AssignRolesCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Matches(@"\S")
                .WithMessage("UserId is Required");
        }
    }
}
