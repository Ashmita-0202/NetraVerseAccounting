using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetTaxStatusInSales
{
    public record GetTaxStatusInSalesResponse
    (
        string companyId,
        bool showTaxInSales
        );
}
