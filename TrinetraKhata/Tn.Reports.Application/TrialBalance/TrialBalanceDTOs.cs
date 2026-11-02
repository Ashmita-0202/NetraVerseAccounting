using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TrialBalance
{
    public record TrialBalanceDTOs
   (
       string ledgerId,
       decimal? debitAmount,
       decimal? creditAmount
       );
}
