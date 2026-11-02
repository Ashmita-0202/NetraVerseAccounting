using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase
{
    public record  UpdateTaxStatusInPurchaseCommand
    (
          string companyId,
        bool showTaxInPurchase
    ):IRequest<Result<UpdateTaxStatusInPurchaseResponse>>;
}
