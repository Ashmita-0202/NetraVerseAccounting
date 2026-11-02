using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails
{
    public class AddPurchaseReturnDetailsCommandValidator: AbstractValidator<AddPurchaseReturnDetailsCommand>
    {
        public AddPurchaseReturnDetailsCommandValidator()
        {
            RuleFor(x => x.netReturnAmount)
              .NotEmpty().WithMessage(" NetReturnAmount is required.");

            RuleFor(x => x.reason)
              .NotEmpty().WithMessage(" Reason is required.");
        }
    }
}
