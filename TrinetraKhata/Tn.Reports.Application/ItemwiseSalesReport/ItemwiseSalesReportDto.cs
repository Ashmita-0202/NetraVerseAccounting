using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.ItemwiseSalesReport
{
    public record  ItemwiseSalesReportDto
   (
        string? startDate,
        string? endDate,
        string? stockCenterId,
        string? itemId,
        string? itemGroupId,
        string? ledgerId


    );
}
