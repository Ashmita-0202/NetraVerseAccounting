using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseDetails
{
    public record SerialAndRateDTOs
   (
        string serialNumber = "",
        decimal rate = 0
        );
}
