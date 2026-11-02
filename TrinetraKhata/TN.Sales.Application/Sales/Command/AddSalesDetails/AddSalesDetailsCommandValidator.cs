using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.AddSalesDetails
{
    public class AddSalesDetailsCommandValidator: AbstractValidator<AddSalesDetailsCommand>
    {
        public AddSalesDetailsCommandValidator()
        {
            RuleFor(x => x.ledgerId)
             .NotEmpty().WithMessage(" Ledger id must be required.");

        }
    }
}
