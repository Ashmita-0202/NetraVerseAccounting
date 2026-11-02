using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.FilterSubledgerGroupByDate
{
    public record  FilterSubledgerGroupDto
    (
        string? name,
        string? startDate,
        string? endDate

    );
}
