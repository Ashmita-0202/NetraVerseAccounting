using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddMenu
{
    public class AddMenuCommandValidator : AbstractValidator<AddMenuCommand>
    {
        public AddMenuCommandValidator()
        {
            RuleFor(x => x.Name).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Menu name is required");

            RuleFor(x => x.TargetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");
        }
    }
}
