using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Inventory;

namespace TN.Reports.Application.StockReport.Queries
{
    public  record StockReportDtos
    (
            string? ledgerId,
            string? stockCenterId,
            string? itemId,
            string? itemGroupId,
            string? companyId,
            List<string>? batchNumbers,          // Only batch number strings
            List<string>? SerialNumbers,
            List<DateTime>? manufactureAndExpiries
    );

}
