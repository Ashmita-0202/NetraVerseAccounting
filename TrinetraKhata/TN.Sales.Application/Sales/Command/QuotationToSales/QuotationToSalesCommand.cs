using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Command.QuotationToSales
{
    public record QuotationToSalesCommand
    (
         string salesQuotationId,
        string? paymentId,
        string? billNumbers,
          string? chequeNumber,
            string? bankName,
            string? accountName

        ) : IRequest<Result<QuotationToSalesResponse>>;


}
