using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetTaxStatusInPurchase
{
    public record GetTaxStatusInPurchaseResponse
    (
          string companyId,
        bool showTaxInPurchase
        );
}
