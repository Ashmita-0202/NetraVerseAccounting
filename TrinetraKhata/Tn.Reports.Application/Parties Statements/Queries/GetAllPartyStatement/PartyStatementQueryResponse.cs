using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.Parties_Statements.Queries
{
    public record PartyStatementQueryResponse
    (
        DateTime dateTime,
        string billNumber,
        string particular,
        decimal debitAmount,
        decimal creditAmount,
        decimal amount,
        string transactionId
        );
}
