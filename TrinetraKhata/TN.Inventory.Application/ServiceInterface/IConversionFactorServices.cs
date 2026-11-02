using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Command.AddConversionFactor;
using TN.Inventory.Application.Inventory.Command.UpdateConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactor;
using TN.Inventory.Application.Inventory.Queries.ConversionFactorById;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.ServiceInterface
{
    public interface IConversionFactorServices
    {
        Task<Result<PagedResult<GetAllConversionFactorQueryResponse>>> GetAllConversionFactor(PaginationRequest paginationRequest, CancellationToken cancellationToken = default);
    
        Task<Result<GetConversionFactorByIdResponse>> GetConversionFactorById(string id,CancellationToken cancellationToken=default);
        Task<Result<AddConversionFactorResponse>> AddConversionFactor(AddConversionFactorCommand addConversionFactorCommand);
        Task<Result<UpdateConversionFactorResponse>> UpdateConversionFactor(string id, UpdateConversionFactorCommand updateConversionFactorCommand);

        Task<Result<PagedResult<FilterConversionFactorByDateQueryResponse>>> GetConversionFactorFilter(PaginationRequest paginationRequest,FilterConversionFactorDTOs filterConversionFactorDTOs);
        Task<Result<bool>> Delete(string id, CancellationToken cancellationToken = default);
    }
}
