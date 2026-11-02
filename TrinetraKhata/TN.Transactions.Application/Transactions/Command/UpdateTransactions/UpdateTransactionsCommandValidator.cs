using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Transactions.Application.Transactions.Command.UpdateTransactions
{
    public  class UpdateTransactionsCommandValidator:AbstractValidator<UpdateTransactionsCommand>
    {
        public UpdateTransactionsCommandValidator() 
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("id is required");
        
        }
    }
}
