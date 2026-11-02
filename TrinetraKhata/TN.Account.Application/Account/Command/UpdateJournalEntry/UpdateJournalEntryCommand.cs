using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry
{
    public record UpdateJournalEntryCommand
    (
            string journalEntryId,
            string referenceNumber,
            string transactionDate,
            string description,
            List<UpdateJournalEntryDetails> journalEntries
    ) :IRequest<Result<UpdateJournalEntryResponse>>;
}
