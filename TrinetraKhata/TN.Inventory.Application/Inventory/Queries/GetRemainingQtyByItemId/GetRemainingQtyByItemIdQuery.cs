using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQtyByItemId;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.GetRemainingQuantityByItemId
{
    public record GetRemainingQtyByItemIdQuery
   (string ItemId) :IRequest<Result<GetRemainingQtyByItemIdQueryResponse>>;
}
