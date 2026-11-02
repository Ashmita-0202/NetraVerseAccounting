using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public class AddPurchaseDetailsCommandValidator : AbstractValidator<AddPurchaseDetailsCommand>
    {
        public AddPurchaseDetailsCommandValidator()
        {
            RuleFor(x => x.ledgerId)
             .NotEmpty().WithMessage(" Ledger id must be required.");
        }
    }
}