using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.AddJournalEntry
{
    public class AddJournalEntityCommandValidator : AbstractValidator<AddJournalEntryCommand>
    {
        public AddJournalEntityCommandValidator()
        {
            //RuleFor(x => x.transactionDate)
            //  .NotEmpty().WithMessage("transactionDate name is required.");
            RuleFor(x => x.description)
              .NotEmpty().WithMessage("description is required.");
        }
    }
}