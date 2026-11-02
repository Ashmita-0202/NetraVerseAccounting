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

namespace TN.Inventory.Application.Inventory.Queries.FilterStockCenter
{
    public class FilterStockCenterQueryHandler: IRequestHandler<FilterStockCenterQuery, Result<PagedResult<FilterStockCenterQueryResponse>>>
    {
        private readonly IStockCenterService _stockCenterService;
        private readonly IMapper _mapper;

        public FilterStockCenterQueryHandler(IStockCenterService stockCenterService,IMapper mapper)
        {
            _stockCenterService = stockCenterService;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterStockCenterQueryResponse>>> Handle(FilterStockCenterQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _stockCenterService.GetFilterStockCenter(request.PaginationRequest, request.FilterStockCenterDto, cancellationToken);

                var filterStockCenter = _mapper.Map<PagedResult<FilterStockCenterQueryResponse>>(result.Data);

                return Result<PagedResult<FilterStockCenterQueryResponse>>.Success(filterStockCenter);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStockCenterQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
