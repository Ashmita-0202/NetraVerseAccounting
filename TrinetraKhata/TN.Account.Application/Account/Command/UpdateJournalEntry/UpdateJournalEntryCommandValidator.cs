using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry
{
   public class UpdateJournalEntryCommandValidator:AbstractValidator<UpdateJournalEntryCommand>
    {
        public UpdateJournalEntryCommandValidator()
        {
            RuleFor(x => x.referenceNumber)
              .NotEmpty().WithMessage("referenceNumber name is required.");
            RuleFor(x => x.transactionDate)
              .NotEmpty().WithMessage("transactionDate name is required.");
            RuleFor(x => x.description)
              .NotEmpty().WithMessage("description is required.");
        }
    }
}
