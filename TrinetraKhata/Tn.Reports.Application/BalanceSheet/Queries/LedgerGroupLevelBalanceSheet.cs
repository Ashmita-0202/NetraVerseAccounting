using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Reports.Application.TrialBalance;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record LedgerGroupLevelBalanceSheet
    (
        string ledgerGroupId,
        string ledgerGroupName,  
        decimal? balance,
        List<SubLedgerGroupLevelBalanceSheet> SubLedgerGroupLevelBalanceSheetResponses
    );
}
