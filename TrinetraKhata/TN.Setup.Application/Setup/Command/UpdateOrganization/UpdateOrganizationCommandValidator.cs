using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Identity.Client;

namespace TN.Setup.Application.Setup.Command.UpdateOrganization
{
    public class UpdateOrganizationCommandValidator:AbstractValidator<UpdateOrganizationCommand>
    {
        public UpdateOrganizationCommandValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required.");



            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");
        }

    }
}
