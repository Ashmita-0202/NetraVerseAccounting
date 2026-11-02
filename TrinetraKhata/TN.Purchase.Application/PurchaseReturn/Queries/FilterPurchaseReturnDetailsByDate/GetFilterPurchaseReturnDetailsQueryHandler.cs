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

namespace TN.Purchase.Application.PurchaseReturn.Queries.FilterPurchaseReturnDetailsByDate
{
    public  class GetFilterPurchaseReturnDetailsQueryHandler:IRequestHandler<GetFilterPurchaseReturnDetailsQuery, Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public GetFilterPurchaseReturnDetailsQueryHandler(IPurchaseDetailsServices purchaseDetailsServices,IMapper mapper)
        {
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;

        }

        public async Task<Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>> Handle(GetFilterPurchaseReturnDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterPurchaseReturnDetails = await _purchaseDetailsServices.GetFilterPurchaseReturnDetails(request.PaginationRequest,request.FilterPurchaseReturnDetailsDtos);

                if (!filterPurchaseReturnDetails.IsSuccess || filterPurchaseReturnDetails.Data == null)
                {
                    return Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>.Failure(filterPurchaseReturnDetails.Message);
                }

                var filterSalesReturnDetailsResult = _mapper.Map<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>(filterPurchaseReturnDetails.Data);

                return Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>.Success(filterSalesReturnDetailsResult);
            }
            catch (Exception ex)
            {
                return Result<PagedResult<GetFilterPurchaseReturnDetailsQueryResponse>>.Failure(
                    $"An error occurred while fetching purchaseReturnDetails  by date: {ex.Message}");
            }
        }
    }
}

