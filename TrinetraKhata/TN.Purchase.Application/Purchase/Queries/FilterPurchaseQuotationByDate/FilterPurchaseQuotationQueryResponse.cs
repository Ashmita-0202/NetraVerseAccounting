using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using static TN.Purchase.Domain.Entities.PurchaseDetails;
using static TN.Shared.Domain.Entities.Purchase.PurchaseQuotationDetails;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseQuotationByDate
{
    public record FilterPurchaseQuotationQueryResponse
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
               
             QuotationStatus QuotationStatus = QuotationStatus.Pending,
               decimal? taxableAmount = 0,
   decimal? subTotalAmount = 0,
            List<PurchaseQuotationItemsDto> PurchaseItems =default
        );

    public record PurchaseQuotationItemsDto
(
    string id,
    decimal quantity,
    string unitId,
    string itemId,
    decimal price,
    decimal amount,
    string createdBy,
    string createdAt,
    string updatedBy,
    string updatedAt,
    string purchaseQuotationDetailsId,
    bool isActive



);
}
