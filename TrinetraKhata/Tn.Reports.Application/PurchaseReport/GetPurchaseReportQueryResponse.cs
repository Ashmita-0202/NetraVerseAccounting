using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Reports.Application.PurchaseReport
{
    public record  GetPurchaseReportQueryResponse
    (
         string ItemId,
        string? itemGroupId,
        List<string> serialNumber,
        string Date,
        string? billNumber,
        string ledgerId,
        decimal quantity,
        decimal price,
        decimal NetAmount,
        decimal? discountAmount,
        decimal? vatAmount,
        decimal grandTotalAmount,
        string stockCenterId
    );

}
