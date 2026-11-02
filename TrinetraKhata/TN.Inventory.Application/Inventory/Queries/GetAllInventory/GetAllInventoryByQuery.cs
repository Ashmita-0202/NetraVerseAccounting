using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.GetAllInventory
{
    public record  GetAllInventoryByQuery
   (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllInventoryByQueryResponse>>>;
}
