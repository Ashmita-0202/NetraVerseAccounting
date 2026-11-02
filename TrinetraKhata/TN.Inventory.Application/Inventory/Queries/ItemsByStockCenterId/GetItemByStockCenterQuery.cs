using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId
{
    public record GetItemByStockCenterQuery(string stockCenterId,PaginationRequest PaginationRequest) : IRequest<Result<PagedResult<GetItemByStockCenterQueryResponse>>>;
   
}
