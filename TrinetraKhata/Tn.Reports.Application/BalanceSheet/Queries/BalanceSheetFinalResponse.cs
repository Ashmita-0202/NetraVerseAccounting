using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.BalanceSheet.Queries
{
    public record BalanceSheetFinalResponse
    (
        List<MasterLevelBalanceSheetResponse> Assets,
        List<MasterLevelBalanceSheetResponse> Liabilities
    );
}
