using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Command.AddPurchaseItems;
using TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.ServiceInterface
{
    public interface IPurchaseItemsServices
    {
        Task<Result<AddPurchaseItemsResponse>> Add(AddPurchaseItemsCommand addPurchaseItemsCommand);
        Task<Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>> GetAllPurchaseItems(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    }
}