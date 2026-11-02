using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TN.Shared.Domain.Abstractions;

namespace TN.receiptDatas.Application.receiptDatas.Command.UpdateReceipt
{
    public class UpdateReceiptCommandValidator :AbstractValidator<UpdateReceiptCommand>
    {
        public UpdateReceiptCommandValidator()
        {
            //RuleFor(x => x.transactionDate)
            // .NotEmpty().WithMessage("transactionDate  is required.");

        }

    }
    
    
}
