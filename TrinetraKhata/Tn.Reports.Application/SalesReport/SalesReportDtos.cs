using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SalesReport
{
    public record  SalesReportDtos
    (
         string? startDate,
         string? endDate,
         string? stockCenterId,
         string? itemGroupId,
         string? billNumber,
         string? ledgerId,
         string? ItemId,
         string? companyId,
         List<string>? SerialNumbers
    );
    
}
