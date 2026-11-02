using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TrialBalance
{
    public record LedgerGroupLevel
    (
        string subLedgerGroupId,
        decimal? debitAmount,
        decimal? creditAmount,
        List<LedgerLevel> ledgersLevels
        );
}
