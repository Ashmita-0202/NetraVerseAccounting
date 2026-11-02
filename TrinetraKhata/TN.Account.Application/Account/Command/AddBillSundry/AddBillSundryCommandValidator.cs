using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.BillSundry
{
    public class AddBillSundryCommandValidator : AbstractValidator<AddBillSundryCommand>
    {

        public AddBillSundryCommandValidator()
        {
            RuleFor(x => x.name)
             .NotEmpty()
             .WithMessage("Name is required.")
             .MaximumLength(100)
             .WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
