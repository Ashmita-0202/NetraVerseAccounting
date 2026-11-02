using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.UpdateSalesDetails.RequestCommandMapper
{
    public static class UpdateSalesDetailsRequestMapper
    {
        public static UpdateSalesDetailsCommand ToCommand(this UpdateSalesDetailsRequest request, string id)
        {
            return new UpdateSalesDetailsCommand
                (
                   id,
                      request.date,
                    request.billNumber,
                    request.ledgerId,
                    request.amountInWords,
                    request.discountPercent,
                    request.discountAmount,
                    request.vatPercent,
                    request.vatAmount,
                    request.grandTotalAmount,
                    request.paymentId,
                    request.referenceNumber,
                    request.subTotalAmount,
                    request.taxableAmount,
                    request.amountAfterVat,
                     request.BillSundryIds,
                    request.salesItems
          
                );
        }
    }
}
