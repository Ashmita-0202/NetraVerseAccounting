using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Purchase.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Purchase.Application.Purchase.Queries.CurrentPurchaseBillNumber
{
    public class CurrentPurchaseBillNumberQueryHandler : IRequestHandler<CurrentPurchaseBillNumberQuery, Result<CurrentPurchaseBillNumberResponse>>
    {
        private readonly IPurchaseDetailsServices _purchaseDetailsServices;

        public CurrentPurchaseBillNumberQueryHandler(IPurchaseDetailsServices purchaseDetailsServices)
        {
            _purchaseDetailsServices = purchaseDetailsServices;

        }
        public async Task<Result<CurrentPurchaseBillNumberResponse>> Handle(CurrentPurchaseBillNumberQuery request, CancellationToken cancellationToken)
        {
            var result = await _purchaseDetailsServices.GetCurrentPurchaseBillNumber();
            if (result.IsSuccess)
            {
                return Result<CurrentPurchaseBillNumberResponse>.Success(new CurrentPurchaseBillNumberResponse(result.Message));
            }
            else
            {
                return Result<CurrentPurchaseBillNumberResponse>.Failure("Bill number is not generated");
            }
        }
    }
}
