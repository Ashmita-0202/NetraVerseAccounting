using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails
{
    public record PurchaseReturnDetailsQueryResponse
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
            string? stockCenterId,
            List<PurchaseReturnItemsDto> PurchaseReturnItems
        );

    public record PurchaseReturnItemsDto
        (
        string id,
            string purchaseReturnDetailsId,
            string purchaseItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalPrice
        );
}
