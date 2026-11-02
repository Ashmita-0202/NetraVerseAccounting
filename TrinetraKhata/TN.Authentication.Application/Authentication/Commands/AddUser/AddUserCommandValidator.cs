using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Authentication.Application.Authentication.Commands.AddUser
{
   public class AddUserCommandValidator:AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator() 
        {
            RuleFor(x => x.UserName)
                          .NotEmpty()
                          .WithMessage("username is required.");


            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.");


        }
    }
}
