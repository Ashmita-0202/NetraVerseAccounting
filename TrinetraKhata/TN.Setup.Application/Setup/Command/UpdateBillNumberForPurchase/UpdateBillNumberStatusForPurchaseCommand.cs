using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using static TN.Authentication.Domain.Entities.Company;

namespace TN.Setup.Application.Setup.Command.UpdateBillNumberForPurchase
{
    public record UpdateBillNumberStatusForPurchaseCommand
    (
          string id,
          BillNumberGenerationType billNumberGenerationTypeForPurchase
        
    ):IRequest<Result<UpdateBillNumberStatusForPurchaseResponse>>;
}
