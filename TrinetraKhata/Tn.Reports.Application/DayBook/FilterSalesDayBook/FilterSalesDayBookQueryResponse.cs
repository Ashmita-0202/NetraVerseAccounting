using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.DayBook.FilterSalesDayBook
{
    public record FilterSalesDayBookQueryResponse
    (

            string? date,
            string? billNumber,
            string ledgerId,
            decimal? vatAmount,
            decimal grandTotalAmount,
            decimal netAmount


    );
}
