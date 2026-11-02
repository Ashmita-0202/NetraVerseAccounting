using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using static TN.Purchase.Domain.Entities.PurchaseDetails;
using static TN.Sales.Domain.Entities.SalesDetails;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate
{
    public record  FilterPurchaseDetailsByDateQueryResponse
    (
            string id="",
            string? date="",
            string? billNumber="",
            string ledgerId="",
            string amountInWords="",
            decimal? discountPercent=0,
            decimal? discountAmount=0,
            decimal? vatPercent=0,
            decimal? vatAmount=0,
            string companyId="",
             decimal grandTotalAmount=0,
             PurchaseStatus status = default,
             string? referenceNumber ="",
             string? StockCenterId = "",  
             decimal? taxableAmount=0,
             decimal? subTotalAmount=0,
            List<PurchaseItemsDto> PurchaseItems=default,
           List<QuantityDetailDto> quantityDetailDtos = default
    );

}
