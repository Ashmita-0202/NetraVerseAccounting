using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Command.UpdatePurchaseReturnDetails
{
    public record  UpdatePurchaseReturnDetailsCommand
   (
            string id,
            string purchaseDetailsId,
            DateTime returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal netReturnAmount,
            string companyId,
            List<UpdatePurchaseReturnItemsDTOs> purchaseReturnItems
    ):IRequest<Result<UpdatePurchaseReturnDetailsResponse>>;
}
