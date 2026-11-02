using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TradingAccount
{
    public record  GetTradingAccountQueryResponse
    (
        List<TradingReportGroup> IncomeGroups,
        List<TradingReportGroup> ExpenseGroups,
        decimal TotalIncome,
        decimal TotalExpense,
        decimal GrossProfitOrGrossLoss
    );
}
