using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterJournalByDate
{
    public record FilterJournalDTOs
    (
        string? description,
        string? startDate,
        string? endDate
    );
}
