using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.StockCenters
{
    public  class GetAllStockCenterQueryHandler:IRequestHandler<GetAllStockCenterQuery,Result<PagedResult<GetAllStockCenterQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IStockCenterService _service;

        public GetAllStockCenterQueryHandler(IStockCenterService service, IMapper mapper)
        {
            
            _mapper = mapper;
            _service = service;
        }

        public async Task<Result<PagedResult<GetAllStockCenterQueryResponse>>> Handle(GetAllStockCenterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStock = await _service.GetAllStockCenter(request.paginationRequest,request.name, cancellationToken);
                var allStockDisplay = _mapper.Map<PagedResult<GetAllStockCenterQueryResponse>>(allStock.Data);
                return Result<PagedResult<GetAllStockCenterQueryResponse>>.Success(allStockDisplay);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all stock", ex);
            }
        }
    }
}
