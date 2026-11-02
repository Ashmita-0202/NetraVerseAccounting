using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateIncomeTransactionNumberTypeCommand
{
    public class UpdateIncomeTransactionNumberTypeCommandValidation:AbstractValidator<UpdateIncomeTransactionNumberTypeCommand>
    {
        public UpdateIncomeTransactionNumberTypeCommandValidation()
        {
            RuleFor(x => x.companyId)
           .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
