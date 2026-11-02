using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;

namespace TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate
{
    public record  GetFilterPurchaseReturnDetailsQueryResponse
    (
             string id,
            string purchaseDetailsId,
            string? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string companyId,
            string ledgerId,
            string? StockCenterId,
               decimal? taxableAmount,
             decimal? subTotalAmount,
            List<PurchaseReturnItemsDto> PurchaseReturnItems

    );
}
