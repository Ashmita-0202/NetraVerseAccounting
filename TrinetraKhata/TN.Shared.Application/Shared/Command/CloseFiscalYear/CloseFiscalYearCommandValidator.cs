using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.CloseFiscalYear
{
    public class CloseFiscalYearCommandValidator: AbstractValidator<CloseFiscalYearCommand>
    {
        public CloseFiscalYearCommandValidator()
        {
            RuleFor(x => x.closedFiscalId)
                .NotEmpty().WithMessage("FiscalYearId  is required.");
        }
    }
}
