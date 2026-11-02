using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NV.Payment.Application.Payment.Command.AddPayment
{
    public class AddPaymentMethodCommandValidator:AbstractValidator<AddPaymentMethodCommand>
    {
        public AddPaymentMethodCommandValidator()
        {
            RuleFor(x=>x.subLedgerGroupsId).NotEmpty().WithMessage(" subLedgerGroupsId is required");
        
        }
    }
}
