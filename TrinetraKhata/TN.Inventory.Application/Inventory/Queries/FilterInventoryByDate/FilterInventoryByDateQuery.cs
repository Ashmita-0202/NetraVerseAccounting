using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate
{
    public record FilterInventoryByDateQuery
    (PaginationRequest paginationRequest, FilterInventoryDtos FilterInventoryDtos ):IRequest<Result<FilterInventoryWithTotals>>;
}
