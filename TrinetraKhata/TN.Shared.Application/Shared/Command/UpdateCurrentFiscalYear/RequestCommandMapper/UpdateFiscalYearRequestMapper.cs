using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear.NewFolder
{
    public static class UpdateFiscalYearRequestMapper
    {
        public static UpdateFiscalYearCommand ToCommand(this UpdateFiscalYearRequest request, string companyId)
        {

            return new UpdateFiscalYearCommand
                (
                        companyId,
                        request.currentFiscalYearId
                );
        }
    }
}
