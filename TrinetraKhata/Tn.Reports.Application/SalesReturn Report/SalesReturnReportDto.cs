using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SalesReturn_Report
{
    public record  SalesReturnReportDto
    (
        string? startDate,
        string? endDate,
        string? stockCentreId,
        string? ledgerId,
        string? salesItemsId,
        string? itemGroupId


    );
}
