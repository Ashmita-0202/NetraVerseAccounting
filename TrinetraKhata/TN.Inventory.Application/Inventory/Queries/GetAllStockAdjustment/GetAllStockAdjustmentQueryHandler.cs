using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.GetAllStockAdjustment
{
    public  class GetAllStockAdjustmentQueryHandler:IRequestHandler<GetAllStockAdjustmentQuery,Result<PagedResult<GetAllStockAdjustmentQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IInventoriesServices _services;

        public GetAllStockAdjustmentQueryHandler(IMapper mapper,IInventoriesServices services)
        {
            _mapper=mapper;
            _services = services;
        }

        public async Task<Result<PagedResult<GetAllStockAdjustmentQueryResponse>>> Handle(GetAllStockAdjustmentQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStockAdjustment = await _services.GetAllStockAdjustment(request.PaginationRequest, cancellationToken);
                var allStockAdjustmentDisplay = _mapper.Map<PagedResult<GetAllStockAdjustmentQueryResponse>>(allStockAdjustment.Data);
                return Result<PagedResult<GetAllStockAdjustmentQueryResponse>>.Success(allStockAdjustmentDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all stock adjustment ", ex);
            }
        }
    }
}
