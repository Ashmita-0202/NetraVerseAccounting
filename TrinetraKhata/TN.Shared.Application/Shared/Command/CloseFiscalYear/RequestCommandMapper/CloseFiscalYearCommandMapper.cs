using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.CloseFiscalYear.RequestCommandMapper
{
    public static class CloseFiscalYearCommandMapper
    {
        public static CloseFiscalYearCommand ToCommand(this CloseFiscalYearRequest request)
        {
            return new CloseFiscalYearCommand
                (
                request.closedFiscalId,
                request.autoOpenNext,
                request.generateClosingEntries
                );
        }
    }
}
