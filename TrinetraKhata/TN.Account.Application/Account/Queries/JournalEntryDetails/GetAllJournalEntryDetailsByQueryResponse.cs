using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.JournalEntryDetails
{
    public record GetAllJournalEntryDetailsByQueryResponse
    (       
            string id,
            string journalEntryId,
            string ledgerId,
            decimal debitAmount,
            decimal creditAmount
     );
}
