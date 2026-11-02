using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Domain.Entities;

namespace TN.Reports.Application.SalesReturn_Report
{
    public record  GetSalesReturnReportQueryResponse
    (
      
            string? returnDate,
             string? salesReturnNumber,
            string ledgerId,
            string salesItemsId,
            string? itemGroupId,
            string unitId,
            decimal returnQuantity,
            decimal returnUnitPrice,
            decimal netReturnAmount,
            decimal? taxAdjustment,
            decimal totalReturnAmount,
            string? stockCenterId

    );
}
