using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Command.AddSalesReturnItemRequest
{
    public record AddSalesReturnItemsRequest
    (
         string salesItemsId = "",
            decimal returnQuantity = 0,
            decimal returnUnitPrice = 0,
            decimal returnTotalAmount = 0,
            string itemsId = ""
        );
    
}
