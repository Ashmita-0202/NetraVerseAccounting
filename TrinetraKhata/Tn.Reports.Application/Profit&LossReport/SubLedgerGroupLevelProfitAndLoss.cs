using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  SubLedgerGroupLevelProfitAndLoss
    (

        string SubLedgerGroupId,
        string SubLedgerGroupName,
        decimal Total,
        List<LedgerLevelProfitAndLoss> Ledgers

    );
}
