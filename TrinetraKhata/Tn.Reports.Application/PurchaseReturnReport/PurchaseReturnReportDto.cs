using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.PurchaseReturnReport
{
    public record  PurchaseReturnReportDto
    (
        string? startDate,
        string? endDate,
        string? stockCenterId,
        string? ledgerId,
        string? purchaseItemsId,
        string? itemGroupId

    );
}
