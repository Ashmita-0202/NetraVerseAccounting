using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Sales.Application.Sales.Command.UdpateBIllNumberGenerationByCompany
{
    public  class UpdateBillNumberGenerationByCompanyCommandValidator:AbstractValidator<UpdateBillNumberGenerationByCompanyCommand>
    {
        public UpdateBillNumberGenerationByCompanyCommandValidator()
        {
            RuleFor(x => x.companyId)
            .NotEmpty().WithMessage(" Company id must be required.");
        }
    }
}
