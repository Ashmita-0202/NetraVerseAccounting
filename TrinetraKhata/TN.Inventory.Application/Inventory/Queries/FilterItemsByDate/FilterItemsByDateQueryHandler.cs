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

namespace TN.Inventory.Application.Inventory.Queries.FilterItemsByDate
{
    public  class FilterItemsByDateQueryHandler:IRequestHandler<FilterItemsByDateQuery,Result<PagedResult<FilterItemsByDateQueryResponse>>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;

        public FilterItemsByDateQueryHandler(IItemsServices itemsServices,IMapper mapper)
        { 
            _itemsServices=itemsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<FilterItemsByDateQueryResponse>>> Handle(FilterItemsByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _itemsServices.GetItemsFilter(request.PaginationRequest, request.FilterItemsDTOs);

                var filterItems = _mapper.Map<PagedResult<FilterItemsByDateQueryResponse>>(result.Data);

                return Result<PagedResult<FilterItemsByDateQueryResponse>>.Success(filterItems);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterItemsByDateQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
