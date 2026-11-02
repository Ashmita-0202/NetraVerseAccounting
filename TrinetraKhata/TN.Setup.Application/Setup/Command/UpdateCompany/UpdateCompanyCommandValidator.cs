using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateCompany
{
   public class UpdateCompanyCommandValidator :AbstractValidator<UpdateCompanyCommand>
    {
        public UpdateCompanyCommandValidator() 
        {
            RuleFor(x => x.name)
                        .NotEmpty()
                        .WithMessage("Institution name is required.");


            RuleFor(x => x.address)
                .NotEmpty()
                .WithMessage("Address is required.");

        }
    }
}
