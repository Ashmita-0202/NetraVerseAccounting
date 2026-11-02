using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TN.Account.Application.Account.Command.UpdateJournalEntryDetails
{
   public class UpdateJournalDetailsCommandValidator:AbstractValidator<UpdateJournalDetailsCommand>
    {
        public UpdateJournalDetailsCommandValidator()
        {
            RuleFor(x => x.id)
               .NotEmpty().WithMessage("JournalEntry Details ID is required.");
        }
    }
}
