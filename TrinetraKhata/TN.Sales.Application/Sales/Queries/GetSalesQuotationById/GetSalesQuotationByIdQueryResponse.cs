using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate;

namespace TN.Sales.Application.Sales.Queries.GetSalesQuotationById
{
    public record  GetSalesQuotationByIdQueryResponse
    (

           string id,
            string? date,
            string? billNumber,
            string ledgerId,
            string amountInWords,
            decimal? discountPercent,
            decimal? discountAmount,
            decimal? vatPercent,
            decimal? vatAmount,
            string companyId,
            decimal grandTotalAmount,
              string? stockCenterId,
            List<SalesQuotationItemsDto> salesItems

    );
}
