using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById
{
    public record GetStockTransferDetailsByIdQuery
  (string id):IRequest<Result<GetStockTransferDetailsByIdQueryResponse>>;
}
