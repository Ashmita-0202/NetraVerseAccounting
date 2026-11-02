using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddUnits;
using TN.Inventory.Application.Inventory.Command.DeleteUnits;
using TN.Inventory.Application.Inventory.Command.UpdateUnits;
using TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate;
using TN.Inventory.Application.Inventory.Queries.Units;
using TN.Inventory.Application.Inventory.Queries.UnitsById;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
   public interface IUnitsServices
    {
        Task<Result<PagedResult<GetAllUnitsByQueryResponse>>> GetAllUnits(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
        Task<Result<GetUnitsByIdQueryResponse>> GetUnitsById(string id, CancellationToken cancellationToken=default);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken=default);
        Task<Result<AddUnitsResponse>> AddUnits(AddUnitsCommand addUnitsCommand);
        Task<Result<UpdateUnitsResponse>> UpdateUnits(string id, UpdateUnitsCommand updateUnitsCommand);
        Task<Result<PagedResult<FilterUnitsByDateQueryResponse>>> GetUnitsFilter(PaginationRequest paginationRequest,FilterUnitsDTOs filterUnitsDTOs);

    }
}
