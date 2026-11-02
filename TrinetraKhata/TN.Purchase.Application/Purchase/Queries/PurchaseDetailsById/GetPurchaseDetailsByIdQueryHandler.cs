using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById
{
  public sealed class GetPurchaseDetailsByIdQueryHandler:IRequestHandler<GetPurchaseDetailsByIdQuery, Result<GetPurchaseDetailsByIdQueryResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public GetPurchaseDetailsByIdQueryHandler(IPurchaseDetailsServices purchaseDetailsServices, IMapper mapper)
        {
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;

        }

        public async Task<Result<GetPurchaseDetailsByIdQueryResponse>> Handle(GetPurchaseDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var purchaseDetails = await _purchaseDetailsServices.GetPurchaseDetailsById(request.id);

                return Result<GetPurchaseDetailsByIdQueryResponse>.Success(purchaseDetails.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Purchase entry Details by Id", ex);

            }
        }
    }
}
