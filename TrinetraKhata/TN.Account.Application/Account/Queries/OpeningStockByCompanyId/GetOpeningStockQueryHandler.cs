using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Account.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.OpeningStockByCompanyId
{
    public class GetOpeningStockQueryHandler : IRequestHandler<GetOpeningStockQuery, Result<GetOpeningStockQueryResponse>>
    {
        private readonly IOpeningStockService _service;

        public GetOpeningStockQueryHandler(IOpeningStockService service) 
        {
            _service = service;


        }

        public async Task<Result<GetOpeningStockQueryResponse>> Handle(GetOpeningStockQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var ledgerGroupById = await _service.GetOpeningStock(request.companyId);

                return Result<GetOpeningStockQueryResponse>.Success(ledgerGroupById.Data);


            }
            catch (Exception ex)

            {

                throw new Exception("An error occurred while fetching openingstock by companyId", ex);

            }
        }
    }
}
