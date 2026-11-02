using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  LedgerGroupLevelProfitAndLoss
    (
      string LedgerGroupId,
        string LedgerGroupName,
        decimal Total,
        List<SubLedgerGroupLevelProfitAndLoss> SubGroups

    );
}
