using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public class AddPurchaseItemsCommandValidator : AbstractValidator<AddPurchaseItemsCommand>
    {
        public AddPurchaseItemsCommandValidator()
        {
            RuleFor(x => x.quantity)
              .NotEmpty().WithMessage(" Quantity is required.");
        }
    }
}