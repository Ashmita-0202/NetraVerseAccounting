using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems
{
    public record class GetAllSalesReturnItemsByQuery
   (PaginationRequest PaginationRequest):IRequest<Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>>;
}
