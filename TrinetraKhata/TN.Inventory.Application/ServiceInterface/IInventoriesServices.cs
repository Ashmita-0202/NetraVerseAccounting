using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddStockAdjustment;
using TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate;
using TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs;
using TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQtyByItemId;
using TN.Inventory.Application.Inventory.Queries.InventoriesLogsById;
using TN.Inventory.Application.Inventory.Queries.InventoryByItem;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IInventoriesServices
    {
         Task<Result<IEnumerable<InventoryByItemQueryResponse>>> GetInventoryItem(string itemId, CancellationToken cancellationToken);
         Task<Result<GetRemainingQtyByItemIdQueryResponse>> GetRemainingQtyByItemId(string ItemId,CancellationToken cancellationToken=default);
         Task<Result<PagedResult<GetAllInventoryByQueryResponse>>> GetAllInventory(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
         Task<Result<GetInventoriesLogsByIdQueryResponse>> GetInventoriesLogsById(string id, CancellationToken cancellationToken=default);
         Task<Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>> GetAllInventoriesLogs(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<FilterInventoryWithTotals>> GetInventoryFilter(PaginationRequest paginationRequest,FilterInventoryDtos filterInventoryDtos, CancellationToken cancellationToken);

        #region Stock Adjustment
        Task<Result<UpdateStockAdjustmentResponse>> UpdateStockAdjustment(string id, UpdateStockAdjustmentCommand command);
        Task<Result<AddStockAdjustmentResponse>> AddStockAdjustment(AddStockAdjustmentCommand command);
       Task<Result<bool>> Delete(string id,CancellationToken cancellationToken=default);
        Task<Result<PagedResult<GetAllStockAdjustmentQueryResponse>>> GetAllStockAdjustment(PaginationRequest paginationRequest,CancellationToken cancellationToken = default);
        Task<Result<PagedResult<FilterStockAdjustmentQueryResponse>>> GetFilterStockAdjustment(PaginationRequest paginationRequest, FilterStockAdjustmentDto filterStockAdjustmentDto, CancellationToken cancellationToken);
        #endregion
    }
}
