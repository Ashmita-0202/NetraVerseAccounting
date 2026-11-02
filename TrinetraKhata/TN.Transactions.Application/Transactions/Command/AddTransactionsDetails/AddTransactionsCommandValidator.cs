using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Transactions.Application.Transactions.Command.AddTransactions
{
    public class AddTransactionsCommandValidator:AbstractValidator<AddTransactionsCommand>
    {
        public AddTransactionsCommandValidator()
        {
            RuleFor(x => x.transactionDate)
               .NotEmpty().WithMessage("transactionDate name is required.");

        }
    }
}
