using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany;

namespace TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany
{
    public class UpdateItemStatusByCompanyCommandValidator: AbstractValidator<UpdateItemStatusByCompanyCommand>
    {
        public UpdateItemStatusByCompanyCommandValidator()
        {
            RuleFor(x => x.companyId)
               .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
