using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.OpeningClosingBalance
{
    public record OpeningClosingBalanceResponse
    (
        string ledgerId,
        string ledgerName,
        decimal openingBalance,
        decimal closingBalance
        );
}
