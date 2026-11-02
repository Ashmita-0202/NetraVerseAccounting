using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.GodownwiseInventoryReport
{
    public record  GodownwiseInventoryDtos
    (

         string? startDate,
        string? endDate,
        string? stockCenterId,
        string? itemId,
        string? itemGroupId,
        string? valuationMethod
      



    );
}
