using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Command.UpdateSalesReturnDetails.RequestCommandMapper
{
    public static class UpdateSalesReturnDetailsRequestMapper
    {
       public static  UpdateSalesReturnDetailsCommand ToCommand(this UpdateSalesReturnDetailsRequest request,string id)
        {
            return new UpdateSalesReturnDetailsCommand
                (
                    id,
                    request.salesDetailsId,
                    request.returnDate,
                    request.totalReturnAmount,
                    request.taxAdjustment,
                    request.netReturnAmount,
                    request.reason,
                    request.createdBy,
                    request.createdAt,
                    request.modifiedBy,
                    request.modifiedAt
                    
                );
        }
    }
}
