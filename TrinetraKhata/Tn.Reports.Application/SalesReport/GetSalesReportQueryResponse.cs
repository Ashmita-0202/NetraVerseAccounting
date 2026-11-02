using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SalesReport
{
    public record GetSalesReportQueryResponse
    (

        
        string ItemId,
        string? itemGroupId,
        List<string> serialNumber,
        string Date,
        string? billNumber,
        string ledgerId,
        decimal quantity,
        decimal price,
        decimal NetAmount,
        decimal? discountAmount,
        decimal? vatAmount,
        decimal grandTotalAmount,
        string stockCenterId

    );
    
}
