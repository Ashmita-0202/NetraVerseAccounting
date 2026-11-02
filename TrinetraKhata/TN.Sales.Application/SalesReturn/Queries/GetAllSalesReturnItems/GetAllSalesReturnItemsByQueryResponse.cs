using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems
{
    public record  GetAllSalesReturnItemsByQueryResponse
    (
            string id,
            string salesReturnDetailsId,
            string salesItemsId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal returnTotalPrice
    );
}
