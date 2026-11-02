using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public record AddPurchaseDetailsResponse
    (
        string Id = "",
        string Date = "",
        string BillNumber = "",
        string LedgerId = "",
        string AmountInWords = "",
        decimal DiscountPercent = 0,
        decimal DiscountAmount = 0,
        decimal VatPercent = 0,
        decimal VatAmount = 0,
        string CreatedBy = "",
        string CompanyId = "",
        DateTime CreatedAt = default,
         decimal grandTotalAmount = 0,
         string paymentId="",
         string referenceNumber="",
         bool isPurchase = false,
         string stockCenterId = "",

             decimal? SubTotalAmount = 0,
            decimal? TaxableAmount = 0,
            decimal? AmountAfterVat = 0,
        List<AddPurchaseItemsRequest>? PurchaseItems = null
        );
}
