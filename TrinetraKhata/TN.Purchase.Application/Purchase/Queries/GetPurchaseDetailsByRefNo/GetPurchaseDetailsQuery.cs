using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo
{
    public record GetPurchaseDetailsQuery
    (string referenceNumber) :IRequest<Result<GetPurchaseDetailsQueryResponse>>;
}
