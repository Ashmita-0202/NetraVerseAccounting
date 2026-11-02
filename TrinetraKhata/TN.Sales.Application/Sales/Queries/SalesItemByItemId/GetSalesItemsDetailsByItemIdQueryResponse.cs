using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.Sales.Queries.SalesItemByItemId
{
    public record GetSalesItemsDetailsByItemIdQueryResponse
    (
        string unitId,
        decimal totalQuantity,
        decimal? rate,
        bool? isVatEnabled,
        bool? isConversionFactor,
        string? conversionFactorId,
        List<string> serialNumbers
        );
}
