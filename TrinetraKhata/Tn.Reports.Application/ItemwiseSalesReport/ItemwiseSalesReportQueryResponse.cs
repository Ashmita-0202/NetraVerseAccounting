using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Domain.Entities;
using TN.Shared.Domain.ExtensionMethod.Pagination;
using static TN.Sales.Domain.Entities.SalesDetails;

namespace TN.Reports.Application.ItemwiseSalesReport
{
    public  record ItemwiseSalesReportQueryResponse
    (
            string? date,
            string? billNumber,
            string ledgerId,
            string? itemId,
            string? itemGroupId,
            string unitId,
            decimal price,
            decimal quantity,
            decimal netAmount,
            decimal? discountAmount,
            decimal? vatAmount,
            decimal grandTotalAmount,
            SalesStatus status,
            string stockCenterId
     
    );
}
