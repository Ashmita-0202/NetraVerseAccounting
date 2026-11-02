using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnDetails
{
    public record AddSalesReturnDetailsResponse
    (
             string id,
            string salesDetailsId,
            DateTime returnDate,
            decimal totalReturnAmount,
            decimal taxAdjustment,
            decimal netReturnAmount,
            string reason,
            string createdBy,
            DateTime createdAt,
            string modifiedBy,
            DateTime modifiedAt

    );
}
