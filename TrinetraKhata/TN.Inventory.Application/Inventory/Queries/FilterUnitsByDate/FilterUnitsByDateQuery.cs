using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate
{
   public record FilterUnitsByDateQuery
    (PaginationRequest PaginationRequest,FilterUnitsDTOs FilterUnitsDTOs):IRequest<Result<PagedResult<FilterUnitsByDateQueryResponse>>>;
}
