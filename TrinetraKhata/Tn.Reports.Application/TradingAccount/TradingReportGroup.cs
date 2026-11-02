using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TradingAccount
{
    public record TradingReportGroup
    (
         string LedgerGroupId,
        List<TradingReportLedgerItem> LedgerItems,
        decimal GroupTotal
    );
}
