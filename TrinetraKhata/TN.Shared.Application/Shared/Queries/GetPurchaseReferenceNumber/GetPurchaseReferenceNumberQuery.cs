using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Queries.GetPurchaseReferenceNumber
{
    public record GetPurchaseReferenceNumberQuery
    (
       string companyId
        ) : IRequest<Result<GetPurchaseReferenceNumberQueryResponse>>;
}
