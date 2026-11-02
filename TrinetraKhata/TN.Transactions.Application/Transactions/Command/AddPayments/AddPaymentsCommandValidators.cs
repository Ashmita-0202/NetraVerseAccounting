using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddPayments
{
    public class AddPaymentsCommandValidators : AbstractValidator<AddPaymentsCommand>
    {
        public AddPaymentsCommandValidators()
        {
            RuleFor(x => x.transactionDate)
              .NotEmpty().WithMessage("transactionDate name is required.");

        }
    }
}
