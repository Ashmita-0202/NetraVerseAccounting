using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear
{
    public record UpdateFiscalYearRequest
    (
        string? currentFiscalYearId
    );
}
