using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Command.AddSalesItems
{
    public record AddSalesItemsRequest
    (
            decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount,
         List<string>? serialNumbers = null
    );
}
