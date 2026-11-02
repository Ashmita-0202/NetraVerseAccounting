using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.FilterItemsByDate;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.FilterUnitsByDate
{
    public class FilterUnitsByDateQueryHandler : IRequestHandler<FilterUnitsByDateQuery, Result<PagedResult<FilterUnitsByDateQueryResponse>>>
    {
        private readonly IUnitsServices _unitsServices;
        private readonly IMapper _mapper;

        public FilterUnitsByDateQueryHandler(IUnitsServices unitsServices, IMapper mapper)
        {
            _unitsServices=unitsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<FilterUnitsByDateQueryResponse>>> Handle(FilterUnitsByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var result = await _unitsServices.GetUnitsFilter(request.PaginationRequest, request.FilterUnitsDTOs);

                var filterUnits = _mapper.Map<PagedResult<FilterUnitsByDateQueryResponse>>(result.Data);

                return Result<PagedResult<FilterUnitsByDateQueryResponse>>.Success(filterUnits);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterUnitsByDateQueryResponse>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
