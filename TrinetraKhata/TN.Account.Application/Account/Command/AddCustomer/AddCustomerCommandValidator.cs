using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.AddCustomer
{
  public class AddCustomerCommandValidator:AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerCommandValidator()
        {
            RuleFor(x => x.fullName)
               .NotEmpty().WithMessage("Ledger name is required.");
        }
    }
}
