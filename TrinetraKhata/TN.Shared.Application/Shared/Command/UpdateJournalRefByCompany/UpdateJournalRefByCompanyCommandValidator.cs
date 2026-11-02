using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany
{
    public  class UpdateJournalRefByCompanyCommandValidator:AbstractValidator<UpdateJournalRefByCompanyCommand>
    {
        public UpdateJournalRefByCompanyCommandValidator()
        {
            RuleFor(x => x.companyId)
             .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
