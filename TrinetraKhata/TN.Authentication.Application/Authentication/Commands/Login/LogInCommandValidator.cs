using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Login
{
    public class LogInCommandValidator : AbstractValidator<LoginCommand>
    {
        public LogInCommandValidator()
        {
            RuleFor(x => x.username)
                .NotEmpty()
                .Matches(@"\S")
                .WithMessage("Username is required");

            RuleFor(x => x.password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
