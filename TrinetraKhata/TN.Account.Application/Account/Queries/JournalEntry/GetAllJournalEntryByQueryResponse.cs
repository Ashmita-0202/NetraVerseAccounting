using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TN.Account.Application.Account.Queries.JournalEntry
{
    public record GetAllJournalEntryByQueryResponse
     (
         string id,
         string referenceNumber,
         string transactionDate,
         string description,
         string createdBy,
         DateTime createdAt,
         string companyId,
         List<JournalEntryDetailsDto> journalEntries
     );

    public record JournalEntryDetailsDto
    (
        string id,
        string ledgerId,
        decimal debitAmount,
        decimal creditAmount
    );
}
