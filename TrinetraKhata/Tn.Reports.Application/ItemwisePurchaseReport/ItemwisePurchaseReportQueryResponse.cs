using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Purchase.Domain.Entities.PurchaseDetails;


namespace TN.Reports.Application.ItemwisePurchaseReport
{
    public record  ItemwisePurchaseReportQueryResponse
    (
            string? date,
            string? billNumber,
            string? referenceNumber,
            string ledgerId,
            string? itemId,
            string? itemGroupId,
            string unitId,
            decimal price,
            decimal quantity,
            decimal netAmount,
            decimal? discountAmount,
            decimal? vatAmount,
            decimal grandTotalAmount,
            PurchaseStatus status,
            string stockCenterId

    );
}
