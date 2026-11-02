using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TN.Account.Application.Account.Command.AddJournalEntryDetails
{
    public class AddJournalEntryDetailsCommandValidator : AbstractValidator<AddJournalEntryDetailsCommand>
    {
        public AddJournalEntryDetailsCommandValidator()
        {

            RuleFor(x => x.debitAmount)
                .NotEmpty()
                .WithMessage("Debit Amount is required.");

            RuleFor(x => x.debitAmount)
                .NotEmpty()
                .WithMessage("Credit Amount is required.");
        }
    }
}
