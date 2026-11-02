using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.Sales.Command.QuotationToSales;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.QuotationToPurchase
{
    public record QuotationToPurchaseCommand
      (
         string purchaseQuotationId,
        string? paymentId,
        string? billNumbers,
        string? chequeNumber,
        string? bankName,
        string? accountName

        ) : IRequest<Result<QuotationToPurchaseResponse>>;
}
