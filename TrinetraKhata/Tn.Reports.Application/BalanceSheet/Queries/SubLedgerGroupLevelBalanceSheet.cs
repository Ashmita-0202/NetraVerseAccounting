using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record SubLedgerGroupLevelBalanceSheet
    (
        string subLedgerGroupId,
        string subLedgerGroupName, 
        decimal? balance,
        List<LedgerLevelBalanceSheet> LedgerLevelBalanceSheetResponses
        );
}
