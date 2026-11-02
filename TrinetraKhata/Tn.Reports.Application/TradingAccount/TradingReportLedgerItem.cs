using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TradingAccount
{
    public record  TradingReportLedgerItem
    (

        string LedgerId,
        string LedgerName,
        decimal Balance

    );
   
}
