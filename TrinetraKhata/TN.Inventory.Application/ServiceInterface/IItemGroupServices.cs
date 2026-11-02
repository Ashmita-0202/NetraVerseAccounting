using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddItemGroup;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate;
using TN.Inventory.Application.Inventory.Queries.ItemGroup;
using TN.Inventory.Application.Inventory.Queries.ItemGroupById;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IItemGroupServices
    {
         Task<Result<PagedResult<GetAllItemGroupByQueryResponse>>> GetAllItemGroup(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetItemGroupByIdQueryResponse>> GetItemGroupById(string id, CancellationToken cancellationToken = default);
        Task<Result<AddItemGroupResponse>> AddItemGroup(AddItemGroupCommand addItemGroupCommand);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default);
        Task<Result<UpdateItemGroupResponse>> UpdateItemGroup(string id, UpdateItemGroupCommand updateItemGroupCommand);
        Task<Result<PagedResult<FilterItemGroupByDateQueryResponse>>> GetItemGroupFilter(PaginationRequest paginationRequest,FilterItemGroupDTOs filterItemGroupDTOs);
    }
}
