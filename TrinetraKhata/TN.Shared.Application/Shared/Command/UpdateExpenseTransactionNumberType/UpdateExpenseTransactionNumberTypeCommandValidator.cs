using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateExpenseTransactionNumberType
{
    public class UpdateExpenseTransactionNumberTypeCommandValidator:AbstractValidator<UpdateExpenseTransactionNumberTypeCommand>
    {
        public UpdateExpenseTransactionNumberTypeCommandValidator()
        {
            RuleFor(x => x.companyId)
           .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
