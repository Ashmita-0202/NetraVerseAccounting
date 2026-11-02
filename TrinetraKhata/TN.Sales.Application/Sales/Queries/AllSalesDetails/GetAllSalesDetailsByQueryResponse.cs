using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Sales.Domain.Entities.SalesDetails;

namespace TN.Sales.Application.Sales.Queries.AllSalesDetails
{
   public record GetAllSalesDetailsByQueryResponse
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
            List<SalesItemsDto> salesItems
   );
    public record SalesItemsDto 
    (   
            string id,
            decimal quantity,
            string unitId,
            string itemId,
            decimal price,
            decimal amount,
            string createdBy,
            string createdAt,
            string updatedBy,
            string updatedAt,
            string hsCode,
                bool? isVatEnabled,
         List<string?> serialNumbers
        

     );
    public record QuantityDetailDto
    (
        string salesItemId,
        string itemId,
        string unitId,
        decimal quantity,
         List<string?> serialNumbers

    );

}
