using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.OpeningClosingBalanceByLedger
{
    public record OpeningClosingBalanceByLedgerResponse
    (
        string ledgerId,
        string ledgerName,
        decimal? openingBalance,
        decimal? closingBalance
        );
}
