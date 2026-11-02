using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment
{
    public record  FilterStockAdjustmentQuery
    (PaginationRequest PaginationRequest,FilterStockAdjustmentDto FilterStockAdjustmentDto):IRequest<Result<PagedResult<FilterStockAdjustmentQueryResponse>>>;
}
