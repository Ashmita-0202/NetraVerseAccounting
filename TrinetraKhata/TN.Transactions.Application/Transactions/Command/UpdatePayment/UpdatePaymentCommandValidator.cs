using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace TN.Transactions.Application.Transactions.Command.UpdatePayment
{
    public  class UpdatePaymentCommandValidator:AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidator()
        {
            //RuleFor(x => x.id)
            //    .NotEmpty().WithMessage("Payment Id is required.")
            //    .NotNull().WithMessage("Payment Id cannot be null.");
        }
    }
}
