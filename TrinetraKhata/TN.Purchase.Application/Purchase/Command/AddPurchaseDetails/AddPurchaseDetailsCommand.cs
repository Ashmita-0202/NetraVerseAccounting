using MediatR;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Shared.Domain.Abstractions;
using static TN.Purchase.Domain.Entities.PurchaseDetails;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public record AddPurchaseDetailsCommand
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
                 decimal? subTotalAmount,
                decimal? taxableAmount,
                decimal? amountAfterVat,
                    
                 List<BillSundryRequestDTOs> BillSundryIds = null!,
            List<AddPurchaseItemsRequest> PurchaseItems=null!)
        : IRequest<Result<AddPurchaseDetailsResponse>>;

}