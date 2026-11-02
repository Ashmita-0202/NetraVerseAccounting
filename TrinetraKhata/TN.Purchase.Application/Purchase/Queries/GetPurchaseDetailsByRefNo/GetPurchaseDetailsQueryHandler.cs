using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.Purchase.Queries.PurchaseDetailsById;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.GetPurchaseDetailsByRefNo
{
    public class GetPurchaseDetailsQueryHandler:IRequestHandler<GetPurchaseDetailsQuery,Result<GetPurchaseDetailsQueryResponse>>
    {
        private readonly IPurchaseDetailsServices _services;
        private readonly IMapper _mapper;

        public GetPurchaseDetailsQueryHandler(IPurchaseDetailsServices services,IMapper mapper)
        {
            _services= services;
            _mapper= mapper;
        }

        public async Task<Result<GetPurchaseDetailsQueryResponse>> Handle(GetPurchaseDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var purchaseData = await _services.GetPurchaseDetailsByRefNo(request.referenceNumber);

                return Result<GetPurchaseDetailsQueryResponse>.Success(purchaseData.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Purchase entry Details by referenceNo", ex);

            }
        }
    }
}
