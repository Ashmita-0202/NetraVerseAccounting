using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using static TN.Purchase.Domain.Entities.PurchaseDetails;


namespace TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById
{
  public record GetPurchaseDetailsByIdQueryResponse
    (
            string id,
            string? date,
            string? billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            string companyId,
             decimal grandTotalAmount,
             PurchaseStatus status,
             string? referenceNumber,
             string paymentId,
             string? StockCenterId,
             string? chequeNumber,
            string? bankName,
            string? accountName,
            string? purchaseQuotationNumber,
                 decimal? subTotalAmount,
                decimal? taxableAmount,
                decimal? amountAfterVat,
                 List<BillSundryRequestDTOs> BillSundryIds,
            List<PurchaseItemsDto> PurchaseItems);
}
