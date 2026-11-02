using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.SalesSummaryReport
{
    public record  GetSalesSummaryQueryResponse
    (

        string itemId,
        string ledgerId,
        string stockCenterId,
        int NumberOfInvoices,
        decimal TotalQuantity,
        decimal TotalGrossAmount,
        decimal TotalDiscount,
        decimal TotalTax,
        decimal TotalNetAmount,
        string TopItem

    );
}
