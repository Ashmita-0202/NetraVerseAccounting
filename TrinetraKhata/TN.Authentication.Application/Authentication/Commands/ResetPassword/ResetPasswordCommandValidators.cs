using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidators : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidators()
        {
            RuleFor(x => x.token)
                .NotEmpty()
                .WithMessage("Token is required");

            RuleFor(x => x.email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email address format");

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
