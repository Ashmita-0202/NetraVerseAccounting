using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate
{
    public record  FilterSalesReturnDetailsDTOs
    (
        string? ledgerId,
        string? startDate,
        string? endDate

    );
}
