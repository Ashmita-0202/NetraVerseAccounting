using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;

namespace TN.Sales.Application.Sales.Queries.GetSalesDetailsByRefNo
{
    public record  GetSalesDetailsQueryResponse
    (
             string id,
            string date,
            string billNumber,
            string ledgerId,
            string amountInWords,
            decimal discountPercent,
            decimal discountAmount,
            decimal vatPercent,
            decimal vatAmount,
            string companyId,
            decimal grandTotalAmount,
            string referenceNumber,
            string? paymentId,
            string? stockCenterId,
            List<SalesItemsDto> salesItems
    );
}
