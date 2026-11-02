using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.UpdatePurchaseDetails
{
   public record UpdatePurchaseDetailsCommand
    (
            string id,
           string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            decimal grandTotalAmount,
            string paymentId,
            string? referenceNumber,
             decimal? subTotalAmount,
                decimal? taxableAmount,
                decimal? amountAfterVat,
                 List<BillSundryRequestDTOs>? BillSundryIds,
            List<UpdatePurchaseItemsDTOs> updatePurchaseItems
   ) :IRequest<Result<UpdatePurchaseDetailsResponse>>;
}
