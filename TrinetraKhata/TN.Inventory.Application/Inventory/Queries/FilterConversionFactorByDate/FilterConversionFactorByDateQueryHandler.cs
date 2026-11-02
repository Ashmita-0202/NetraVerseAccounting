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

namespace TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate
{
    public  class FilterConversionFactorByDateQueryHandler:IRequestHandler<FilterConversionFactorByDateQuery,Result<PagedResult<FilterConversionFactorByDateQueryResponse>>>
    {
        private readonly IConversionFactorServices _conversionFactor;
        private readonly IMapper _mapper;

        public FilterConversionFactorByDateQueryHandler(IConversionFactorServices conversionFactorServices,IMapper mapper)
        {
            _conversionFactor=conversionFactorServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<FilterConversionFactorByDateQueryResponse>>> Handle(FilterConversionFactorByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterConversionFactor = await _conversionFactor.GetConversionFactorFilter(request.PaginationRequest,request.FilterConversionFactorDTOs);

                if (!filterConversionFactor.IsSuccess || filterConversionFactor.Data == null)
                {
                    return Result<PagedResult<FilterConversionFactorByDateQueryResponse>>.Failure(filterConversionFactor.Message);
                }

                var filterItemGroupResult = _mapper.Map<PagedResult<FilterConversionFactorByDateQueryResponse>>(filterConversionFactor.Data);

                return Result<PagedResult<FilterConversionFactorByDateQueryResponse>>.Success(filterItemGroupResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterConversionFactorByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching Conversion factor  by date: {ex.Message}");
            }
        }
    }
}
