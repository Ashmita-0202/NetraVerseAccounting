using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NV.Payment.Application.Payment.Command.UpdatePayment
{
    public class  UpdatePaymentMethodCommandValidator:AbstractValidator<UpdatePaymentMethodCommand>
    {
        public UpdatePaymentMethodCommandValidator()
        {
            RuleFor(x=>x.subLedgerGroupsId).NotEmpty();
        }
    }
}
