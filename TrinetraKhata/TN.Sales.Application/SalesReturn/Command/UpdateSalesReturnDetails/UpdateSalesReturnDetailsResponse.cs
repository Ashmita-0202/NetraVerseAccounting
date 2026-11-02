using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails
{
    public record  UpdateSalesReturnDetailsResponse
    (

         string id,
         string salesDetailsId,
         string? returnDate,
         decimal totalReturnAmount,
         decimal? taxAdjustment,
         decimal netReturnAmount,
         string reason,
         string createdBy,
         DateTime createdAt,
         string modifiedBy,
         DateTime modifiedAt
    );
}
