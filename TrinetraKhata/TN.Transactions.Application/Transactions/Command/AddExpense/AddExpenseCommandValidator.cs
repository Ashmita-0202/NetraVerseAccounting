using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Command.AddExpense
{
    public class AddExpenseCommandValidator : AbstractValidator<AddExpenseCommand>
    {
        public AddExpenseCommandValidator()
        {
            RuleFor(x => x.expensesNumber)
              .NotEmpty().WithMessage("ExpensesNumber name is required.");
        }
    }
}
