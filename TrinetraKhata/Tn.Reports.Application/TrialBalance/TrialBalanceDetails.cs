using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TrialBalance
{
    public record TrialBalanceDetails
   (
       string masterId,
        string subLedgerGroupId,
       string ledgerId,
       decimal? debitAmount,
       decimal? creditAmount
       );
}
