using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Purchase.Domain.Entities.PurchaseDetails;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById
{
    public record  GetPurchaseQuotationByIdQueryResponse
    (

          string id = "",
            string? date = "",
            string? billNumber = "",
            string ledgerId = "",
            string amountInWords = "",
            decimal? discountPercent = 0,
            decimal? discountAmount = 0,
            decimal? vatPercent = 0,
            decimal? vatAmount = 0,
            string companyId = "",
             decimal grandTotalAmount = 0,
             string? referenceNumber = "",
             string? StockCenterId = "",
            List<PurchaseQuotationItemsDto> PurchaseItems = default
    );
}
