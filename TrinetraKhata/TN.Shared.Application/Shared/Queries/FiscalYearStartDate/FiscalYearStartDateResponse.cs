using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.FiscalYearStartDate
{
    public record FiscalYearStartDateResponse
    (
        string runningFiscalYearStartDate,
        string openingCompanyFiscalYearStartDate
        );
}
