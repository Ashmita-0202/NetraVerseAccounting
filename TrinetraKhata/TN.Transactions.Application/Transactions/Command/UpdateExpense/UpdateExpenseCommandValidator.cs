using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Transactions.Application.Transactions.Command.UpdateExpense
{
    public class UpdateExpenseCommandValidator:AbstractValidator<UpdateExpenseCommand>
    {
        public UpdateExpenseCommandValidator()
        {
            
        }
    }
}
