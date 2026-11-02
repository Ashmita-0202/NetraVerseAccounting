using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.GetCurrentPurchaseReferenceNumber
{
    public class CurrentPurchaseRefNoQueryHandler:IRequestHandler<CurrentPurchaseRefNoQuery,Result<CurrentPurchaseRefNoQueryResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;
        private readonly IMapper _mapper;

        public CurrentPurchaseRefNoQueryHandler(IPurchaseDetailsServices purchaseDetailsServices,IMapper mapper)
        {
            _purchaseDetailsServices=purchaseDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<CurrentPurchaseRefNoQueryResponse>> Handle(CurrentPurchaseRefNoQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseDetailsServices.GetCurrentPurchaseReferenceNumber();
            if (result.IsSuccess)
            {
                return Result<CurrentPurchaseRefNoQueryResponse>.Success(new CurrentPurchaseRefNoQueryResponse(result.Message));
            }
            else
            {
                return Result<CurrentPurchaseRefNoQueryResponse>.Failure("Reference number is not generated");
            }
        }
    }
}
