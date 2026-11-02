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

namespace TN.Inventory.Application.Inventory.Queries.FilterStockAdjustment
{
    public class FilterStockAdjustmentQueryHandler : IRequestHandler<FilterStockAdjustmentQuery, Result<PagedResult<FilterStockAdjustmentQueryResponse>>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public FilterStockAdjustmentQueryHandler(IInventoriesServices inventoriesServices,IMapper mapper)
        {
            _inventoriesServices = inventoriesServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterStockAdjustmentQueryResponse>>> Handle(FilterStockAdjustmentQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _inventoriesServices.GetFilterStockAdjustment(request.PaginationRequest, request.FilterStockAdjustmentDto,cancellationToken);

                var filterStockAdjustment = _mapper.Map<PagedResult<FilterStockAdjustmentQueryResponse>>(result.Data);

                return Result<PagedResult<FilterStockAdjustmentQueryResponse>>.Success(filterStockAdjustment);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStockAdjustmentQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
