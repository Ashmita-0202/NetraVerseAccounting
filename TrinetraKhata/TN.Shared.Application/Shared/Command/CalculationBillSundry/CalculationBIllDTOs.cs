using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Command.CalculationBillSundry
{
    public record CalculationBIllDTOs
  (
        string billSundryId,
        decimal? subTotalAmount,
        decimal? taxableAmount,
        decimal? amountAfterVat
        );
}
