using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Reports.Application.StockDetailReport.Queries
{
    public record GetStockDetailReportResponse
    (
          string itemId,
        string UnitId,

        decimal OpeningStockQty,
        decimal OpeningStockAvgPrice,
        decimal OpeningStockAmount,

        decimal PurchaseQty,
        decimal PurchaseAvgPrice,
        decimal PurchaseAmountIn,

        decimal SalesQty,
        decimal SalesAvgPrice,
        decimal SalesAmountOut,

        decimal PurchaseReturnQty,
        decimal PurchaseReturnAvgPrice,
        decimal PurchaseReturnAmount,

        decimal SalesReturnQty,
        decimal SalesReturnAvgPrice,
        decimal SalesReturnAmount,

           decimal BalanceQty,
        decimal BalanceAvgPrice,
        decimal BalanceAmount,

        string stockAdjustmentValue

    );
}
