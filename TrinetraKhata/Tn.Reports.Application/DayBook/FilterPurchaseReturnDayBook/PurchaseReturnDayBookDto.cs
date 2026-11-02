using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook
{
    public record  PurchaseReturnDayBookDto
    (
        string? startDate,
        string? endDate

    );
}
