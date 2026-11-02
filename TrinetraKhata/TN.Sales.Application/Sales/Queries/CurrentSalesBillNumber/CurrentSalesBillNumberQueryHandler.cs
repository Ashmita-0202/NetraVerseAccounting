using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Sales.Application.Sales.Queries.CurrentSalesBillNumber
{
    public class CurrentSalesBillNumberQueryHandler : IRequestHandler<CurrentSalesBillNumbersQuery, Result<CurrentSalesBillNumberResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;

        public CurrentSalesBillNumberQueryHandler(ISalesDetailsServices salesDetailsServices)
        {
            _salesDetailsServices = salesDetailsServices;

        }
        public async Task<Result<CurrentSalesBillNumberResponse>> Handle(CurrentSalesBillNumbersQuery request, CancellationToken cancellationToken)
        {
            var result = await _salesDetailsServices.GetCurrentSalesBillNumber();
            if (result.IsSuccess)
            {
                return Result<CurrentSalesBillNumberResponse>.Success(new CurrentSalesBillNumberResponse(result.Message));
            }
            else
            {
                return Result<CurrentSalesBillNumberResponse>.Failure("Bill number is not generated");
            }
        }
    }
}
