using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Authentication.Application.Authentication.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                       .NotEmpty()
                       .WithMessage("Institution name is required.");


            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.");
        }

    }
}
