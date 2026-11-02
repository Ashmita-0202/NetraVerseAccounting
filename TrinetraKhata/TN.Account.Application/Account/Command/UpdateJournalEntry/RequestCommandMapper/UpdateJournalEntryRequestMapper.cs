using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateJournalEntry.RequestCommandMapper
{
    public static class UpdateJournalEntryRequestMapper
    {
        public static UpdateJournalEntryCommand ToCommand(this UpdateJournalEntryRequest request, string id) 
        { 
            return new UpdateJournalEntryCommand
                (
                    id,
                    request.referenceNumber,
                    request.transactionDate,
                    request.description,
                    request.journalEntries
                );
        }
    }
}
