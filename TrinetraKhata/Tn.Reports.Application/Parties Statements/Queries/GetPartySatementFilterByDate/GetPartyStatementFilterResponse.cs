using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Parties_Statements.Queries.GetPartySatementFilterByDate
{
    public record GetPartyStatementFilterResponse
    (
        DateTime dateTime,
        string billNumber,
        string affectedLedgerId,
        string paymentMethodId,
        string referenceNumber,
        string transactions,
        decimal debitAmount,
        decimal creditAmount,
        decimal amount,
        string transactionId

    );
}
