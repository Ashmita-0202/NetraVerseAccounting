using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateMenu
{
    public class UpdateMenuCommandValidator : AbstractValidator<UpdateMenuCommand>
    {
        public UpdateMenuCommandValidator()
        {
            RuleFor(x => x.name).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Menu name is required");



            RuleFor(x => x.targetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");
        }
    }
}
