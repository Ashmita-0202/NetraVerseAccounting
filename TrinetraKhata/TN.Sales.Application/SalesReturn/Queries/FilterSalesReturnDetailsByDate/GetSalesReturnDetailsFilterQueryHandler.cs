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


namespace TN.Sales.Application.SalesReturn.Queries.FilterSalesReturnDetailsByDate
{
    public class GetSalesReturnDetailsFilterQueryHandler:IRequestHandler<GetSalesReturnDetailsFilterQuery,Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;

        public GetSalesReturnDetailsFilterQueryHandler(ISalesReturnServices salesReturnServices,IMapper mapper)
        {
            _salesReturnServices=salesReturnServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>> Handle(GetSalesReturnDetailsFilterQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSalesReturnDetails = await _salesReturnServices.SalesReturnDetailsFilters(request.PaginationRequest,request.filterSalesReturnDetailsDTOs);

                if (!filterSalesReturnDetails.IsSuccess || filterSalesReturnDetails.Data == null)
                {
                    return Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>.Failure(filterSalesReturnDetails.Message);
                }

                var filterSalesReturnDetailsResult = _mapper.Map<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>(filterSalesReturnDetails.Data);

                return Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>.Success(filterSalesReturnDetailsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetSalesReturnDetailsFilterQueryResponse>>.Failure(
                    $"An error occurred while fetching salesReturnDetails  by date: {ex.Message}");
            }
        }
    }
}
