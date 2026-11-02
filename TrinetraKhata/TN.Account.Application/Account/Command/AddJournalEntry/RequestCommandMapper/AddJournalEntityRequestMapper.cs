using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Command.AddJournalEntryDetails;

namespace TN.Account.Application.Account.Command.AddJournalEntry.RequestCommandMapper
{
    public static class AddJournalEntityRequestMapper
    {
        public static AddJournalEntryCommand ToCommand(this AddJournalEntryRequest request)
        {
            return new AddJournalEntryCommand
                (
                request.referenceNumber,
                request.transactionDate,
                request.description,
                request.journalEntries

                );
        }
    }
}
