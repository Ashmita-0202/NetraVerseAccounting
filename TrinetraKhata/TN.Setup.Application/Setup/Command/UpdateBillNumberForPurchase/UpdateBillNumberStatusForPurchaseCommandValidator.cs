using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
    public class UpdateBillNumberStatusForPurchaseCommandValidator:AbstractValidator<UpdateBillNumberStatusForPurchaseCommand>
    {
        public UpdateBillNumberStatusForPurchaseCommandValidator()
        {
            RuleFor(x => x.id)
                        .NotEmpty()
                        .WithMessage("company id is required.");

        }
    }
}
