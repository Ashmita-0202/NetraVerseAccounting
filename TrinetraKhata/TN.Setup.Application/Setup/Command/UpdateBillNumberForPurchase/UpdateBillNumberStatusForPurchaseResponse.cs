using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
    public record UpdateBillNumberStatusForPurchaseResponse
    (
         string id,
          BillNumberGenerationType billNumberGenerationTypeForPurchase
    );
}
