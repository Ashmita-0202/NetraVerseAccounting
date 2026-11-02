using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails
{
    public  class UpdatePurchaseReturnDetailsCommandValidator:AbstractValidator<UpdatePurchaseReturnDetailsCommand>
    {
        public UpdatePurchaseReturnDetailsCommandValidator()
        {
            RuleFor(x=>x.purchaseDetailsId).NotEmpty().WithMessage("purchaseDetailsId is required");
        }
    }
}
