using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.Purchase.Queries.FilterPurchaseDetailsByDate
{
    public class FilterPurchaseDetailsByDateQueryHandler : IRequestHandler<FilterPurchaseDetailsByDateQuery, Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public FilterPurchaseDetailsByDateQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>> Handle(FilterPurchaseDetailsByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                
                var filterPurchaseDetails = await _purchaseDetailsServices.GetPurchaseDetailsFilter(request.PaginationRequest,request.FilterPurchaseDetailsDTOs);

             
                if (!filterPurchaseDetails.IsSuccess || filterPurchaseDetails.Data == null)
                {
                    return Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>.Failure(filterPurchaseDetails.Message);
                }

                var filterPurchaseDetailsResult= _mapper.Map<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>(filterPurchaseDetails.Data);

                return Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>.Success(filterPurchaseDetailsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<FilterPurchaseDetailsByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching purchaseDetails  by date: {ex.Message}");
            }
        }
    }
}
