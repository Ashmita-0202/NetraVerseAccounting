using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.Sales.Queries.GetAllSalesItems
{
    public record  GetAllSalesItemsByQuery
    (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllSalesItemsByQueryResponse>>>;
    
}
