using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails
{
    public class UpdatePurchaseDetailsCommandValidator:AbstractValidator<UpdatePurchaseDetailsCommand>
    {
        public UpdatePurchaseDetailsCommandValidator() 
        {
            RuleFor(x => x.ledgerId)
                 .NotEmpty().WithMessage(" Ledger id must be required.");

        }
    }
}
