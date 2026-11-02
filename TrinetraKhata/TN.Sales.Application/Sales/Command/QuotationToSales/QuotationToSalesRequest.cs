
namespace TN.Sales.Application.Sales.Command.QuotationToSales
{
    public record QuotationToSalesRequest
    (
        string salesQuotationId,
        string? paymentId,
        string? billNumbers,
          string? chequeNumber,
  string? bankName,
  string? accountName

        );
    
}
