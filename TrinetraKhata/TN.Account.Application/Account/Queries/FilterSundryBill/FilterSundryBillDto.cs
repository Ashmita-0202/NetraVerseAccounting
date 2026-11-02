using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterSundryBill
{
    public record FilterSundryBillDto
    (

        string? startDate,
        string? endDate
    );
}
