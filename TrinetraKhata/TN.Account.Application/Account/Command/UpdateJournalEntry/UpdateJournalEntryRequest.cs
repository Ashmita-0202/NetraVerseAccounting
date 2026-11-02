using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry
{
   public record UpdateJournalEntryRequest
    (
            string referenceNumber,
            string transactionDate,
            string description,
            List<UpdateJournalEntryDetails> journalEntries
    );


}
