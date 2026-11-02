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

namespace TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnDetails
{
    public class GetAllSalesReturnDetailsByQueryHandler : IRequestHandler<GetAllSalesReturnDetailsByQuery, Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;

        public GetAllSalesReturnDetailsByQueryHandler(ISalesReturnServices salesReturnServices, IMapper mapper)
        {
            _salesReturnServices = salesReturnServices;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>> Handle(GetAllSalesReturnDetailsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesReturnDetails = await _salesReturnServices.GetAllSalesReturnDetails(request.PaginationRequest, cancellationToken);
                var allSalesReturnDetailsDisplay = _mapper.Map<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>(allSalesReturnDetails.Data);

                return Result<PagedResult<GetAllSalesReturnDetailsByQueryResponse>>.Success(allSalesReturnDetailsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all salesReturn Details ", ex);
            }
        }
    }
}
