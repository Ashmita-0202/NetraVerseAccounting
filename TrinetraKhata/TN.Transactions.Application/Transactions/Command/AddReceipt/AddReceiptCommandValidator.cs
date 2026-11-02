using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddReceipt
{
    public class AddReceiptCommandValidator : AbstractValidator<AddReceiptCommand>
    {
        public AddReceiptCommandValidator()
        {
            RuleFor(x => x.transactionDate)
              .NotEmpty().WithMessage("transactionDate name is required.");
        }
    }
}
