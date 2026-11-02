using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.SalesReturn.Queries.GetSalesReturnDetailsById
{
    public record GetSalesReturnDetailsByIdQuery
  (string id) :IRequest<Result<GetSalesReturnDetailsByIdQueryResponse>>;
}
