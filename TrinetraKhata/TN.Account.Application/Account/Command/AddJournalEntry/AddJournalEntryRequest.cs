using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;

namespace TN.Account.Application.Account.Command.AddJournalEntry
{
    public record AddJournalEntryRequest
    (
        string? referenceNumber,
        string? transactionDate,
        string description,
        List<AddJournalEntryDetailsRequest> journalEntries
        );
}
