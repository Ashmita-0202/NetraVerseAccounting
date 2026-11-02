using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record LedgerLevelBalanceSheet
    (
         string ledgerId,
        string ledgerName,   
        decimal? balance,
        string balanceType
        );
    
}
