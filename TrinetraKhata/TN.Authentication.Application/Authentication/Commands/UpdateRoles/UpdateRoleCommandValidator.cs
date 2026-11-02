using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Authentication.Application.Authentication.Commands.UpdateRoles
{
   public class UpdateRoleCommandValidator:AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidator() 
        {
            RuleFor(x => x.Name)
                      .NotEmpty()
                      .WithMessage("Role name is required.");


        }
    }
}
