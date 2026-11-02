using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterLedgerByDate
{
    public record FilterLedgerDto
    (
        string? name,
        string? startDate,
        string? endDate
    );
}
