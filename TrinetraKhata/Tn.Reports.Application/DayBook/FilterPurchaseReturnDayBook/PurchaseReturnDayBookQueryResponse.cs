using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.DayBook.FilterPurchaseReturnDayBook
{
    public record  PurchaseReturnDayBookQueryResponse
    (
            string? returnDate,
              string? billNumber,
            string ledgerId,
            decimal netReturnAmount,
            decimal? taxAdjustment,
            decimal totalReturnAmount
             
           


    );
}
