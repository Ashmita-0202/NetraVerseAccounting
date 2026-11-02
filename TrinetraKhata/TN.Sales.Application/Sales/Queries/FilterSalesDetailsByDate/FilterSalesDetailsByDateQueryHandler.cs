using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.Sales.Queries.FilterSalesDetailsByDate
{
    public  class FilterSalesDetailsByDateQueryHandler:IRequestHandler<FilterSalesDetailsByDateQuery, Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public FilterSalesDetailsByDateQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>> Handle(FilterSalesDetailsByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSalesDetails = await _salesDetailsServices.GetFilterSalesDetails(request.PaginationRequest,request.FilterSalesDetailsDTOs);

                if (!filterSalesDetails.IsSuccess || filterSalesDetails.Data == null)
                {
                    return Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>.Failure(filterSalesDetails.Message);
                }

                var filterSalesResult = _mapper.Map<PagedResult<FilterSalesDetailsByDateQueryResponse>>(filterSalesDetails.Data);

                return Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>.Success(filterSalesResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterSalesDetailsByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching sales details  by date: {ex.Message}");
            }
        }
    }
}
