using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT
{
    public record VATDetailsDTOs
    (
           string journalEntryId,
           decimal? debitAmount,
           decimal? creditAmount
    );
}
