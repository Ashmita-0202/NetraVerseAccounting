using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdatePaymentTransactionNumberType
{
    public class UpdatePaymentTransactionNumberTypeCommandValidator : AbstractValidator<UpdatePaymentTransactionNumberTypeCommand>
    {
        public UpdatePaymentTransactionNumberTypeCommandValidator()
        {
            RuleFor(x => x.companyId)
           .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
