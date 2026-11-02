using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.Purchase.Queries.Purchase;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails
{
    public class PurchaseReturnDetailsQueryHandler : IRequestHandler<PurchaseReturnDetailsByQueries, Result<PagedResult<PurchaseReturnDetailsQueryResponse>>>
    {
        private readonly IMapper _mapper;
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;


        public PurchaseReturnDetailsQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper)
        {
            _mapper = mapper;
            _purchaseDetailsServices = purchaseDetailsServices;

        }


        public async Task<Result<PagedResult<PurchaseReturnDetailsQueryResponse>>> Handle(PurchaseReturnDetailsByQueries request, CancellationToken cancellationToken)
        {
            try
            {
                var allPurchaseReturnDetails = await _purchaseDetailsServices.GetAllPurchaseReturnDetails(request.PaginationRequest, cancellationToken);
                var allPurchaseReturnDetailsDisplay = _mapper.Map<PagedResult<PurchaseReturnDetailsQueryResponse>>(allPurchaseReturnDetails.Data);

                return Result<PagedResult<PurchaseReturnDetailsQueryResponse>>.Success(allPurchaseReturnDetailsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchase return details ", ex);
            }
        }
    }
}
