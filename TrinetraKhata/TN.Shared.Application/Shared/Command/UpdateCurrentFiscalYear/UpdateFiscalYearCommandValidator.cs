using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear
{
    public class UpdateFiscalYearCommandValidator:AbstractValidator<UpdateFiscalYearCommand>
    {
        public UpdateFiscalYearCommandValidator()
        {
            RuleFor(x => x.currentFiscalYearId)
                .NotEmpty().WithMessage("companyId  is required.");
                
        }
    }
}
