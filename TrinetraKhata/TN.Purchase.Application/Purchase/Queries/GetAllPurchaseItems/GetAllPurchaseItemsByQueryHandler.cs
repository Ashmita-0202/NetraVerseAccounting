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

namespace TN.Purchase.Application.Purchase.Queries.GetAllPurchaseItems
{
    public class GetAllPurchaseItemsByQueryHandler:IRequestHandler<GetAllPurchaseItemsByQuery,Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>>
    {
        private readonly IPurchaseItemsServices _purchaseItemsServices;
        private readonly IMapper _mapper;

        public GetAllPurchaseItemsByQueryHandler(IPurchaseItemsServices purchaseItemsServices,IMapper mapper)
        {
            _purchaseItemsServices=purchaseItemsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>> Handle(GetAllPurchaseItemsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allPurchaseItems = await _purchaseItemsServices.GetAllPurchaseItems(request.PaginationRequest, cancellationToken);
                var allPurchaseItemsDisplay = _mapper.Map<PagedResult<GetAllPurchaseItemsByQueryResponse>>(allPurchaseItems.Data);

                return Result<PagedResult<GetAllPurchaseItemsByQueryResponse>>.Success(allPurchaseItemsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchase Items", ex);
            }
        }
    }
}
