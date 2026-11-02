using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TN.Setup.Application.Setup.Command.AddModule;

namespace TN.Setup.Application.Setup.Command.AddOrganization
{
    public class AddOrganizationCommandValidator: AbstractValidator<AddOrganizationCommand>
    {

        public AddOrganizationCommandValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required.");
           


            RuleFor(x => x.address)
                .NotEmpty().WithMessage("Address is required.");
                


        }
    }
}
