using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.ItemwiseProfitReport
{
    public record  ItemwiseProfitDtos
    (
         string? startDate,
        string? endDate,
        string? stockCenterId,
        string? itemId,
        string? itemGroupId


    );
}
