using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.CalculationBillSundry
{
    public record CalculationBillResponseDTOs
  (
      string BillSundryId,
      decimal CalculatedAmount,
      decimal? CostOfGoodsInSale,
      decimal? CostOfGoodsInPurchase,
      decimal? CostOfGoodsInStockTransfer);
}
