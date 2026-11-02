using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.AddLedgerGroup
{
    public class AddLedgerGroupCommandValidator: AbstractValidator<AddLedgerGroupCommand>
    {
        public AddLedgerGroupCommandValidator()
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
