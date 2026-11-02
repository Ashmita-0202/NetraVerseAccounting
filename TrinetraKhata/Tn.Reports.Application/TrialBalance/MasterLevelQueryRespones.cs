using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TrialBalance
{
    public record MasterLevelQueryRespones
    (
        string masterId,
        decimal? debitAmount,
        decimal? creditAmount,
        List<LedgerGroupLevel> ledgerGroupLevels
        );
}
