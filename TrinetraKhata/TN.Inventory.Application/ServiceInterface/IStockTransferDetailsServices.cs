using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddStockCenter;
using TN.Inventory.Application.Inventory.Command.AddStockTransferDetails;
using TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.ConversionFactor;
using TN.Inventory.Application.Inventory.Queries.FilterStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetAllStockTransferDetails;
using TN.Inventory.Application.Inventory.Queries.GetStockTransferDetailsById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IStockTransferDetailsServices
    {
        Task<Result<PagedResult<FilterStockTransferQueryResponse>>> FilterStockTransferDetails(PaginationRequest paginationRequest, FilterStockTransferDetailsDto filterStockTransferDetailsDto);
        Task<Result<AddStockTransferDetailsResponse>> Add(AddStockTransferCommand command);
        Task<Result<PagedResult<GetAllStockTransferDetailsQueryResponse>>> GetAllStockTransferDetail(PaginationRequest paginationRequest,CancellationToken cancellationToken=default);
        Task<Result<bool>> Delete(string id,CancellationToken cancellationToken);
    Task<Result<GetStockTransferDetailsByIdQueryResponse>> GetStockTransferDetailsById(string id,CancellationToken cancellationToken=default);
    Task<Result<UpdateStockTransferDetailsResponse>> Update(string id, UpdateStockTransferDetailsCommand command);

    }
}
