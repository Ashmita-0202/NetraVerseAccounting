using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  ProfitAndLossDetails
    (
        string SectionName,     // Income or Expense
        string LedgerGroupId,
        string LedgerGroupName,
        string SubLedgerGroupId,
        string SubLedgerGroupName,
        string LedgerId,
        string LedgerName,
        decimal Balance,
        string BalanceType


    );
   
}
