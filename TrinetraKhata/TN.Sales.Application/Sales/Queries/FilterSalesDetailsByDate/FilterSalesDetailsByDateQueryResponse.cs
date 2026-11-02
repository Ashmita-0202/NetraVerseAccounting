using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using static TN.Sales.Domain.Entities.SalesDetails;

namespace TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate
{
    public record  FilterSalesDetailsByDateQueryResponse
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
            SalesStatus salesStatus,
            decimal grandTotalAmount,
              string? paymentId,
              string? stockCenterId,
                decimal? taxableAmount,
             decimal? subTotalAmount,
            List<SalesItemsDto> salesItems,
            List<QuantityDetailDto> quantityDetailDtos
    );
}
