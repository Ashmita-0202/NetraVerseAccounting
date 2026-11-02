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

namespace TN.Purchase.Application.Purchase.Queries.Purchase
{
  public class GetAllPurchaseDetailsQueryHandler: IRequestHandler<GetAllPurchaseDetailsByQueries,Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>>
        {
        private readonly IMapper _mapper;
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;

        public GetAllPurchaseDetailsQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper) 
        {
            _mapper = mapper;
            _purchaseDetailsServices=purchaseDetailsServices;

        }

        public async Task<Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>> Handle(GetAllPurchaseDetailsByQueries request, CancellationToken cancellationToken)
        {
            try
            {
                var allPurchaseDetails = await _purchaseDetailsServices.GetAllPurchaseDetails(request.PaginationRequest, cancellationToken);
                var allPurchaseDetailsDisplay = _mapper.Map<PagedResult<GetAllPurchaseDetailsQueryResponse>>(allPurchaseDetails.Data);

                return Result<PagedResult<GetAllPurchaseDetailsQueryResponse>>.Success(allPurchaseDetailsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all purchaseDetails ", ex);
            }
        }
    }
}
