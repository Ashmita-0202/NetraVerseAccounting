using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.UpdateStockCenter;
using TN.Inventory.Application.Inventory.Queries.FilterStockCenter;
using TN.Inventory.Application.Inventory.Queries.StockCenters;
using TN.Inventory.Application.Inventory.Queries.StockCentersById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IStockCenterService
    {
        Task<Result<PagedResult<GetAllStockCenterQueryResponse>>> GetAllStockCenter(PaginationRequest paginationRequest, string? name, CancellationToken cancellationToken=default);
        Task<Result<AddStockCenterResponse>> Add(AddStockCenterCommand command);
        Task<Result<bool>> Delete(string id,CancellationToken cancellationToken=default);
       Task<Result<UpdateStockCenterResponse>> Update(string id,UpdateStockCenterCommand command);
    Task<Result<PagedResult<FilterStockCenterQueryResponse>>> GetFilterStockCenter(PaginationRequest paginationRequest, FilterStockCenterDto filterStockCenterDto, CancellationToken cancellationToken = default);
        Task<Result<GetStockQueryByIdResponse>> GetStockCenterById(string id, CancellationToken cancellationToken = default);
    }
}
