using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateBillSundry
{
    public class UpdateBillSundryCommandValidator:AbstractValidator<UpdateBillSundryCommand>
    {
        public UpdateBillSundryCommandValidator()
        {
            RuleFor(x => x.Id)
             .NotEmpty().WithMessage(" Id is required.");
        }

    }
    
}
