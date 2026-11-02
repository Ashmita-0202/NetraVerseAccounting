using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnItems;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.AddPurchaseReturnDetails
{
    public record AddPurchaseReturnDetailsCommand
    (
        string purchaseDetailsId,
            string returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal discount,
            decimal netReturnAmount,
            string reason,
            string paymentId,
            string? purchaseReturnNumber,
                        string? chequeNumber,
string? bankName,
string? accountName,
     decimal? subTotalAmount,
                decimal? taxableAmount,
                decimal? amountAfterVat,
                 List<BillSundryRequestDTOs>? BillSundryIds,
           List<AddPurchaseReturnItemsRequest> PurchaseReturnItems

        ) : IRequest<Result<AddPurchaseReturnDetailsResponse>>;
}
