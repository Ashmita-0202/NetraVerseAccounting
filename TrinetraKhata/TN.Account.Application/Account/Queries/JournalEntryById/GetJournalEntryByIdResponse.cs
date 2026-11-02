using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.JournalEntryById
{
    public record GetJournalEntryByIdResponse
   (
             string id,
            string referenceNumber,
            string transactionDate,
            string description,
            List<JournalEntryDetailsByIdDto> journalEntries
         );

    public record JournalEntryDetailsByIdDto
(
    string id,
    string ledgerId,
    decimal debitAmount,
    decimal creditAmount
);
}
