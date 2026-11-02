using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Command.AddPurchaseItems
{
    public record AddPurchaseItemsCommand
    (
          decimal quantity,
          string unitId,
          string itemId,
          decimal price,
          decimal amount
    //string purchaseDetailsId
    ) : IRequest<Result<AddPurchaseItemsResponse>>;
}