using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear
{
    public record GetSelectedFiscalYearQueryResponse
   (
        string Id = "",
        string FyName = ""
        );
}
