using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.QuotationToPurchase
{
    public class QuotationToPurchaseCommandValidator : AbstractValidator<QuotationToPurchaseCommand>
    {
        public QuotationToPurchaseCommandValidator()
        {
            RuleFor(x => x.purchaseQuotationId)
               .NotEmpty().WithMessage("Purchase Quotation Id must be required.");
            RuleFor(x => x.paymentId)
                .NotEmpty().WithMessage("Payment Id must be required.");

        }
    }
}
