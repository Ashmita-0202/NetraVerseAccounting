using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Transactions.Application.Transactions.Command.UpdateIncome
{
    public class UpdateIncomeCommandValidator:AbstractValidator<UpdateIncomeCommand>
    {
        public UpdateIncomeCommandValidator()
        {
            RuleFor( x => x.id).NotEmpty().WithMessage("Id is required");
        }
    }
}
