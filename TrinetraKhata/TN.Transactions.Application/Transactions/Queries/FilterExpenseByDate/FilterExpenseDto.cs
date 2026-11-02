using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Transactions.Application.Transactions.Queries.FilterExpenseByDate
{
    public record  FilterExpenseDto
   (
          string? narration,
         string? startDate,
         string? endDate

    );
}
