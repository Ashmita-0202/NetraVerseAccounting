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

namespace TN.Inventory.Application.Inventory.Queries.FilterStockTransferDetails
{
    public class FilterStockTransferDetailsQueryHandler:IRequestHandler<FilterStockTransferDetailsQuery, Result<PagedResult<FilterStockTransferQueryResponse>>>
    {
        private readonly IStockTransferDetailsServices _services;
        private readonly IMapper _mapper;

        public FilterStockTransferDetailsQueryHandler(IStockTransferDetailsServices services,IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterStockTransferQueryResponse>>> Handle(FilterStockTransferDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _services.FilterStockTransferDetails(request.PaginationRequest, request.FilterStockTransferDetailsDto);

                var filterStockTransferDetails = _mapper.Map<PagedResult<FilterStockTransferQueryResponse>>(result.Data);

                return Result<PagedResult<FilterStockTransferQueryResponse>>.Success(filterStockTransferDetails);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterStockTransferQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
