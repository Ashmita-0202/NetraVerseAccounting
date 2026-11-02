using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.CurrentPurchaseBillNumber
{
    public record CurrentPurchaseBillNumberQuery
    (
        ):IRequest<Result<CurrentPurchaseBillNumberResponse>>;
}
