
using TN.Sales.Application.Sales.Queries.AllSalesDetails;
using static TN.Shared.Domain.Entities.Sales.SalesQuotationDetails;

namespace TN.Sales.Application.Sales.Queries.FilterSalesQuotationByDate
{
    public record FilterSalesQuotationQueryResponse
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
              QuotationStatus? QuotationStatus,
                  decimal? taxableAmount,
   decimal? subTotalAmount,
            List<SalesQuotationItemsDto> salesItems
        );

    public record SalesQuotationItemsDto
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
        string salesQuotationDetailsId,
        bool isActive



 );
}
