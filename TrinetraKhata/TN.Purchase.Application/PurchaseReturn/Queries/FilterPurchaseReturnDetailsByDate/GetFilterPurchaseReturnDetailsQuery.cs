using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate
{
    public record  GetFilterPurchaseReturnDetailsQuery
  (PaginationRequest PaginationRequest,FilterPurchaseReturnDetailsDtos FilterPurchaseReturnDetailsDtos):IRequest<Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>>;
}
