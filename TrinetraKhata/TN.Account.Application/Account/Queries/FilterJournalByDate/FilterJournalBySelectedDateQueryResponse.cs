using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Application.Account.Queries.JournalEntry;

namespace TN.Account.Application.Account.Queries.FilterJournalByDate
{
    public record  FilterJournalBySelectedDateQueryResponse
   (        
            string id="",
            string referenceNumber = "",
            string transactionDate = "",
            string description="",
            string createdBy = "",
            string companyId="",
            DateTime createdAt= default,
            string modifiedBy = "",
            DateTime modifiedAt = default,
         List<JournalEntryDetailsDto> journalEntries = default
    );
}
