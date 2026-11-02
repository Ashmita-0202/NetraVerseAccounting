using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.TradingAccount
{
    public record TradingAccountDto
    (
        decimal OpeningStock,
        decimal Purchases,
        decimal PurchaseReturns,
        decimal Sales,
        decimal SalesReturns,
        decimal ClosingStock,
        decimal DirectExpenses,
        decimal GrossProfit,
        decimal GrossLoss


    );
}
