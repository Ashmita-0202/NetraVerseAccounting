using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
  public record UpdateBillNumberStatusForPurchaseRequest
  (
          string id,
          BillNumberGenerationType billNumberGenerationTypeForPurchase
  );
}