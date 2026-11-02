using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.FilterCompanyByDate
{
    public record FilterCompanyDTOs
     (
        string name,
        string startDate,
        string endDate

    );
}
