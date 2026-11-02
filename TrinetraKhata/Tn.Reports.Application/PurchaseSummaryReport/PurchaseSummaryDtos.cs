using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.PurchaseSummaryReport
{
    public record PurchaseSummaryDtos
    (
         string? startDate,
         string? endDate,
         string? itemId,
         string? itemGroupId,
         string? ledgerId,
        string? stockCenterId

    );
}
