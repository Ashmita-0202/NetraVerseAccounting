using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseQuotationById
{
    public record  GetPurchaseQuotationByIdQuery
   (string id):IRequest<Result<GetPurchaseQuotationByIdQueryResponse>>;
}
