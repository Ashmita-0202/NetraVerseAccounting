using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.ImportExcelForLedgers
{
    public class LedgerExcelCommandValidator:AbstractValidator<LedgerExcelCommand>
    {
        public LedgerExcelCommandValidator() 
        {
            RuleFor(x => x.formFile)
               .NotEmpty().WithMessage("Ledger name is required.");

        }
    }
}
