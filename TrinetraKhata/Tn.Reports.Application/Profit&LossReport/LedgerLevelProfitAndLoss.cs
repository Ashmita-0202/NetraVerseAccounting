using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  LedgerLevelProfitAndLoss
    (
        string LedgerId,
        string LedgerName,
        decimal Balance,
        string BalanceType


    );
}
