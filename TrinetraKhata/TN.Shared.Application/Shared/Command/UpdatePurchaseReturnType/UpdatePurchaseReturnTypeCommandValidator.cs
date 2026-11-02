using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType
{
    public  class UpdatePurchaseReturnTypeCommandValidator:AbstractValidator<UpdatePurchaseReturnTypeCommand>
    {
        public UpdatePurchaseReturnTypeCommandValidator()
        {
            RuleFor(x => x.companyId)
              .NotEmpty().WithMessage("CompanyId is required.");
        }
    }
}
