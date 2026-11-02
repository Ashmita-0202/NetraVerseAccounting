using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterInventoryByDate
{
    public class FilterInventoryByDateQueryHandler : IRequestHandler<FilterInventoryByDateQuery, Result<FilterInventoryWithTotals>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public FilterInventoryByDateQueryHandler(IInventoriesServices inventoriesServices, IMapper mapper)
        { 
        
            _inventoriesServices=inventoriesServices;
            _mapper=mapper;
        
        }

        public async Task<Result<FilterInventoryWithTotals>> Handle(FilterInventoryByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterConversionFactor = await _inventoriesServices.GetInventoryFilter(request.paginationRequest, request.FilterInventoryDtos, cancellationToken);

                if (!filterConversionFactor.IsSuccess || filterConversionFactor.Data == null)
                {
                    return Result<FilterInventoryWithTotals>.Failure(filterConversionFactor.Message);
                }

                var filterItemGroupResult = _mapper.Map<FilterInventoryWithTotals>(filterConversionFactor.Data);

                return Result<FilterInventoryWithTotals>.Success(filterItemGroupResult);
            }
            catch (Exception ex)
            {
                return Result<FilterInventoryWithTotals>.Failure(
                    $"An error occurred while fetching Inventories  by date: {ex.Message}");
            }
        }
    }
}
