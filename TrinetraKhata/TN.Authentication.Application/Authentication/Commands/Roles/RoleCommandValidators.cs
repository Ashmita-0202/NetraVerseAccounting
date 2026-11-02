using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Roles
{
    public class RoleCommandValidators : AbstractValidator<RoleCommand>
    {
        public RoleCommandValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches(@"\S")
                .WithMessage("Rolename is required");
        }
    }
}
