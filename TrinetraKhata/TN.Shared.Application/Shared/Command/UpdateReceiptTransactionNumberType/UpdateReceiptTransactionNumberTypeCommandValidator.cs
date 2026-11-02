using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateReceiptTransactionNumberType
{
    public class UpdateReceiptTransactionNumberTypeCommandValidator : AbstractValidator<UpdateReceiptTransactionNumberTypeCommand>
    {
        public UpdateReceiptTransactionNumberTypeCommandValidator()
        {
            RuleFor(x => x.companyId)
            .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
