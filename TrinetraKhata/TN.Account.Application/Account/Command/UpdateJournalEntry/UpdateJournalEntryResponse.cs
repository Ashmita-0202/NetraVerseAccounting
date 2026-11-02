using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry
{
    public record UpdateJournalEntryResponse
    (
            string id,
            string referenceNumber,
            string transactionDate,
            string description,
            List<UpdateJournalEntryDetails> journalEntries

    );

    public record UpdateJournalEntryDetails
        (
        string id,
        string ledgerId,
        decimal debitAmount,
        decimal creditAmount
        );
}
