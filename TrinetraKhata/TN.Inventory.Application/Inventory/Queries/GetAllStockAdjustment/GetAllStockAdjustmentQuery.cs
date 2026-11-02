using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment
{
    public record  GetAllStockAdjustmentQuery
    (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllStockAdjustmentQueryResponse>>>;
}
