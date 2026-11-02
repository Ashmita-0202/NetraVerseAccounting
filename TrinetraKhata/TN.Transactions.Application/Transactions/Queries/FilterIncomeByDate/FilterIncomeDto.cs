using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Queries.FilterIncomeByDate
{
    public record  FilterIncomeDto
    (
         string? narration,
         string? startDate,
         string? endDate

    );
}
