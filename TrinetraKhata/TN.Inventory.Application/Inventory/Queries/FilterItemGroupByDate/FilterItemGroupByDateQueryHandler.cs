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

namespace TN.Inventory.Application.Inventory.Queries.FilterItemGroupByDate
{
   public class  FilterItemGroupByDateQueryHandler:IRequestHandler<FilterItemGroupByDateQuery,Result<PagedResult<FilterItemGroupByDateQueryResponse>>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;

        public FilterItemGroupByDateQueryHandler(IItemGroupServices itemGroupServices, IMapper mapper)
        {
            _itemGroupServices= itemGroupServices;
            _mapper= mapper;

        }

        public async Task<Result<PagedResult<FilterItemGroupByDateQueryResponse>>> Handle(FilterItemGroupByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _itemGroupServices.GetItemGroupFilter(request.PaginationRequest, request.FilterItemGroupDTOs);

                var filterItemGroup = _mapper.Map<PagedResult<FilterItemGroupByDateQueryResponse>>(result.Data);

                return Result<PagedResult<FilterItemGroupByDateQueryResponse>>.Success(filterItemGroup);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterItemGroupByDateQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
