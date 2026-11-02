using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Transactions.Application.Transactions.Command.ImportExcelForReceipt
{
    public  class ReceiptExcelCommandValidator:AbstractValidator<ReceiptExceCommand>
    {
        public ReceiptExcelCommandValidator()
        {
            RuleFor(x => x.formFile)
                .NotEmpty().WithMessage("form file is required.");
        }
    }
}
