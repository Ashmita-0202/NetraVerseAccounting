using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.FilterMenuByDate
{
    public record FilterMenuDTOs
    (
        string name,
        string startDate,
        string endDate
    );
}
