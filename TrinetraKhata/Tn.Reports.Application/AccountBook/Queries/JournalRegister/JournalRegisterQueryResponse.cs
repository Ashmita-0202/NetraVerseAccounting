using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.JournalEntry;

namespace TN.Reports.Application.AccountBook.Queries.JournalRegister
{
   public record JournalRegisterQueryResponse
    (
            string id,
            string journalEntryId,
            string ledgerId,
            decimal debitAmount,
            decimal creditAmount,
            DateTime transactionDate,
            string companyId
          
   );
}
