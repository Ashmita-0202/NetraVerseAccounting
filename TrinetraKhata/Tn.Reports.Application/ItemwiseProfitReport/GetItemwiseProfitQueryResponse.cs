using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.ItemwiseProfitReport
{
    public record  GetItemwiseProfitQueryResponse
    (
           string stockCenterId,
         string itemId,
          string? itemGroupId,
          string? unitId,
           decimal salesQty,
          decimal price,
          decimal amount,
          decimal grossProfitAmount,
         decimal grossProfitRate
    );
}
