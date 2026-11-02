using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.PurchaseReturnReport
{
    public record  GetPurchaseReturnReportQueryResponse
    (
           string? returnDate,
             string? purchaseReturnNumber,
            string ledgerId,
            string purchaseItemsId,
            string? itemGroupId,
            string unitId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal netReturnAmount,
            decimal? taxAdjustment,
            decimal totalReturnAmount,
            string? stockCenterId

    );
}
