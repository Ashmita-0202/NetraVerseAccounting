using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.VATDetails.Queries.PurchaseAndSalesVAT
{
    public record PurchaseAndSalesVATQueryResponse
    (
        string type,
        string ledgerId,
        decimal? vatOutWards,
        decimal? vatInWards,
        string billNumbers
        );
}
