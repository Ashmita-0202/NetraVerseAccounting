using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.SalesItemByItemId
{
    public record GetSalesItemDetailsByItemIdQuery
    ( string itemsId
        ): IRequest<Result<GetSalesItemsDetailsByItemIdQueryResponse>>;
}
