using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddIncome
{
    public class AddIncomeCommandValidator : AbstractValidator<AddIncomeCommand>
    {
        public AddIncomeCommandValidator()
        {
            //RuleFor(x => x.transactionDate)
            //  .NotEmpty().WithMessage("transactionDate name is required.");
        }
    }
}
