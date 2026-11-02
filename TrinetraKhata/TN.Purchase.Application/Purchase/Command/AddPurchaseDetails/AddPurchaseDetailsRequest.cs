
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public record AddPurchaseDetailsRequest
    (
              string? date,
              string? billNumber,
              string ledgerId,
              string amountInWords,
              decimal? discountPercent, 
              decimal? discountAmount,
              decimal? vatPercent,
              decimal? vatAmount,
              decimal grandTotalAmount,
              string paymentId,
              string? referenceNumber,
              bool isPurchase,
              string? stockCenterId,
                string? chequeNumber,
                string? bankName,
                string? accountName,
                string? purchaseQuotationNumber,
                    decimal? SubTotalAmount,
            decimal? TaxableAmount,
            decimal? AmountAfterVat,
                  List<BillSundryRequestDTOs> BillSundryIds = null!,
              List<AddPurchaseItemsRequest> PurchaseItems = null!
    );
}