using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.AddSalesDetails;
using TN.Sales.Application.SalesReturn.Command.AddSalesReturnItemRequest;

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails
{
    public record AddSalesReturnDetailsRequest
    (

             string salesDetailsId,
             string returnDate,
             decimal totalReturnAmount,
             decimal taxAdjustment,
             decimal discount,
             decimal netReturnAmount,
             string reason,
             string paymentId,
             string? salesReturnNumber,
                                     string? chequeNumber,
string? bankName,
string? accountName,
      decimal? subTotalAmount,
         decimal? taxableAmount,
         decimal? amountAfterVat,
          List<BillSundryRequestDTOs>? BillSundryIds,
           List<AddSalesReturnItemsRequest> SalesReturnItems
    );
}
