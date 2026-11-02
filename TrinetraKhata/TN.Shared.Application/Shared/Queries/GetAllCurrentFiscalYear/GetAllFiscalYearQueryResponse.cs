using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear
{
    public record  GetAllFiscalYearQueryResponse
    (
                string Id = "",
                string FyName = "",
                DateTime StartDate = default,
                DateTime EndDate = default,
                bool IsActive = false
    );
}
