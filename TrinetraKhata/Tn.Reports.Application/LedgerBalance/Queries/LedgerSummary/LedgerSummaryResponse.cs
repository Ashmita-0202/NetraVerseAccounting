using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.LedgerBalance.Queries.LedgerSummary
{
    public record LedgerSummaryResponse
    (
        string JournalEntryId,
        string ledgerId,
        decimal? debitAmount,  
        decimal? creditAmout
        );
}
