using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.Inventory.Command.ImportExcelForItems;
using TN.Inventory.Application.Inventory.Command.UpdateItem;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId;
using TN.Inventory.Application.Inventory.Queries.StockExpiryNotification;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IItemsServices
    {

        Task<Result<PagedResult<StockExpiryNotificationResponse>>> GetStockExpiryNotification(PaginationRequest paginationRequest,CancellationToken cancellationToken = default);

        Task<Result<ItemsExcelResponse>> AddItemsExcel(IFormFile formFile, CancellationToken cancellationToken = default);
        Task<Result<PagedResult<GetAllItemByQueryResponse>>> GetAllItems(PaginationRequest paginationRequest, CancellationToken cancellationToken=default);
        Task<Result<PagedResult<GetItemByStockCenterQueryResponse>>> GetItemByStockCenter(string stockCenterId,PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetItemByIdResponse>> GetItemById(string id, CancellationToken cancellationToken = default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken=default);
        Task<Result<AddItemResponse>> AddItem(AddItemCommand addItemCommand);
        Task<Result<PagedResult<FilterItemsByDateQueryResponse>>> GetItemsFilter(PaginationRequest paginationRequest,FilterItemsDTOs filterItemsDTOs);
        Task<Result<UpdateItemResponse>> UpdateItem(string id, UpdateItemCommand updateItemCommand);
        
    }
}
