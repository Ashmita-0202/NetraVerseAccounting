using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Profit_LossReport
{
    public record  MasterLevelProfitAndLoss
   (
         string SectionName,  // Income / Expense
        string MasterId,
        string MasterName,
        decimal Total,
        List<LedgerGroupLevelProfitAndLoss> LedgerGroups

    );
}
