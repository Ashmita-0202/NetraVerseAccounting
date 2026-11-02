using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.OpeningStockByCompanyId
{
    public record OpeningStockItemDto
    (
             string ItemName,
             decimal Quantity,
             decimal CostPrice,
             decimal TotalValue
    );
}
