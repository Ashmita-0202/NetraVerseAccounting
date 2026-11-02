using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.GetSalesQuotationById
{
    public record  GetSalesQuotationByIdQuery
  (string id):IRequest<Result<GetSalesQuotationByIdQueryResponse>>;
}
