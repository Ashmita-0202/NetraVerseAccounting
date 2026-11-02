using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddModule
{
    public class AddModuleCommandValidator : AbstractValidator<AddModuleCommand>
    {
        public AddModuleCommandValidator() 
        {
            RuleFor(x => x.Name).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Module name is required");

            RuleFor(x => x.Rank).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Role name is required");
            RuleFor(x => x.IconUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("IconUrl is required");

            RuleFor(x => x.TargetUrl).
                NotEmpty()
                .Matches(@"\S")
                .WithMessage("Target url is required");



        }
    }
}
