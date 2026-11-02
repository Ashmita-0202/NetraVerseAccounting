using AutoMapper;
using MediatR;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.PurchaseReturn.Queries.AllPurchaseReturnDetails;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.PurchaseReturn.Queries.PurchaseReturnDetailsById
{
    public sealed class PurchaseReturnDetailsByIdQueryHandler : IRequestHandler<PurchaseReturnDetailsByIdQueries, Result<PurchaseReturnDetailsByIdQueryResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public PurchaseReturnDetailsByIdQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper)
        {
            _purchaseDetailsServices = purchaseDetailsServices;
            _mapper = mapper;

        }
        public async Task<Result<PurchaseReturnDetailsByIdQueryResponse>> Handle(PurchaseReturnDetailsByIdQueries request, CancellationToken cancellationToken)
        {
            try
            {

                var purchaseDetails = await _purchaseDetailsServices.GetPurchaseReturnDetailsById(request.id);

                return Result<PurchaseReturnDetailsByIdQueryResponse>.Success(purchaseDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching PurchaseReturn entry Details by {request.id}", ex);

            }
        }
    }
}
