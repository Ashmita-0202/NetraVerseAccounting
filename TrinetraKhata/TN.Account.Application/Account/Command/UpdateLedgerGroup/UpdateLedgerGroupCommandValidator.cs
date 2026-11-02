using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateLedgerGroup
{
    public class UpdateLedgerGroupCommandValidator : AbstractValidator<UpdateLedgerGroupCommand>
    {
        public UpdateLedgerGroupCommandValidator()
        {
            RuleFor(x => x.name)
                    .NotEmpty()
                    .Matches(@"\S")
                    .WithMessage("Ledger Group name is required.")
                    .MaximumLength(100)
                    .WithMessage("Ledger Group name cannot exceed 100 characters.");

        }

    }
}
