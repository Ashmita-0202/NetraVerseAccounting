using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateInstitution
{
   public class UpdateInstitutionCommandValidator:AbstractValidator<UpdateInstitutionCommand>
    {
        public UpdateInstitutionCommandValidator() 
        
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
