using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.UpdateBillNumberGenerationByCompany
{
    public class UpdateBillNumberGeneratorByCompanyCommandValidator: AbstractValidator<UpdateBillNumberGeneratorByCompanyCommand>
    {
        public UpdateBillNumberGeneratorByCompanyCommandValidator()
        {
            RuleFor(x => x.companyId)
             .NotEmpty().WithMessage(" Company id must be required.");
        }
    }

}
