using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate
{
    public record  FilterPurchaseReturnDetailsDtos
    (
        string? ledgerId,
        string? startDate,
        string? endDate

    );
}
