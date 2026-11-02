using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Sales.Application.Sales.Queries.AllSalesDetails
{
   public class GetAllSalesDetailsByQueryHandler:IRequestHandler<GetAllSalesDetailsByQuery, Result<PagedResult<GetAllSalesDetailsByQueryResponse>>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public GetAllSalesDetailsByQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper) 
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;

        }

        public async Task<Result<PagedResult<GetAllSalesDetailsByQueryResponse>>> Handle(GetAllSalesDetailsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesDetails = await _salesDetailsServices.GetAllSalesDetails(request.PaginationRequest, cancellationToken);
                var allsalesDetailsDisplay = _mapper.Map<PagedResult<GetAllSalesDetailsByQueryResponse>>(allSalesDetails.Data);

                return Result<PagedResult<GetAllSalesDetailsByQueryResponse>>.Success(allsalesDetailsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all sales Details ", ex);
            }
        }
    }
}
