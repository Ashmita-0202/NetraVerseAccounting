using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateLedger
{
    public class UpdateLedgerCommandValidator : AbstractValidator<UpdateLedgerCommand>
    {
        public UpdateLedgerCommandValidator()
        {

           
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Ledger ID is required.")
                .Matches(@"\S").WithMessage("Ledger ID must not be whitespace.");


            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Ledger name is required.");
               




           

        }
    }
}
