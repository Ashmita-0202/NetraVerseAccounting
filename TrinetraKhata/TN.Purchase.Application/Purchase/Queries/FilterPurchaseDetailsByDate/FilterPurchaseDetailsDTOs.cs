using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate
{
    public record FilterPurchaseDetailsDTOs
    (
         string? ledgerId,
        string? startDate,
        string? endDate
    );
}
