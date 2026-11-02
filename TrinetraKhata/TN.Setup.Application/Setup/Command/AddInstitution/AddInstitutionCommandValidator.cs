using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;


namespace TN.Setup.Application.Setup.Command.AddInstitution
{
    public class AddInstitutionCommandValidator: AbstractValidator<AddInstitutionCommand>
    {
        public AddInstitutionCommandValidator()
        {
            RuleFor(x => x.name)
                    .NotEmpty()
                    .WithMessage("Institution name is required.")
                    .Matches(@"\S")
                    .WithMessage("Institution name must not contain only whitespace.");

            RuleFor(x => x.address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .Matches(@"\S")
                .WithMessage("Address must not contain only whitespace.");

        }
    }
}
