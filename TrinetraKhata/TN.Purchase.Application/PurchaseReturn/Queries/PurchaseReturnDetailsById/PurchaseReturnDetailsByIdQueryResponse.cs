using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;

namespace TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById
{
    public record PurchaseReturnDetailsByIdQueryResponse
    (
         string id,
            string purchaseDetailsId,
            string? returnDate,
            decimal totalReturnAmount,
            decimal? taxAdjustment,
            decimal? discount,
            decimal netReturnAmount,
            string companyId,
            string? stockCenterId,
                                    string? chequeNumber,
string? bankName,
string? accountName,
             List<PurchaseReturnItemsDto> PurchaseReturnItems

        );
}
