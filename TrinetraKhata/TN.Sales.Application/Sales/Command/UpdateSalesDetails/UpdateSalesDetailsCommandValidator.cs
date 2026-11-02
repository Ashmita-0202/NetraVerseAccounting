using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.UpdateSalesDetails
{
    public class UpdateSalesDetailsCommandValidator: AbstractValidator<UpdateSalesDetailsCommand>
    {
        public UpdateSalesDetailsCommandValidator()
        {
            RuleFor(x => x.ledgerId)
             .NotEmpty().WithMessage(" Ledger id must be required.");
        }
    }
}
