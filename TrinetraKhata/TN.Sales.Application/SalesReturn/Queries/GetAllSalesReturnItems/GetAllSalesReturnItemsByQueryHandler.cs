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

namespace TN.Sales.Application.SalesReturn.Queries.GetAllSalesReturnItems
{
    public class GetAllSalesReturnItemsByQueryHandler:IRequestHandler<GetAllSalesReturnItemsByQuery, Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>>
    {
        private readonly ISalesReturnServices _salesReturnServices;
        private readonly IMapper _mapper;

        public GetAllSalesReturnItemsByQueryHandler(ISalesReturnServices salesReturnServices,IMapper mapper)
        {
            _salesReturnServices = salesReturnServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>> Handle(GetAllSalesReturnItemsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesReturnItems = await _salesReturnServices.GetAllSalesReturnItems(request.PaginationRequest, cancellationToken);
                var allSalesReturnItemsDisplay = _mapper.Map<PagedResult<GetAllSalesReturnItemsByQueryResponse>>(allSalesReturnItems.Data);

                return Result<PagedResult<GetAllSalesReturnItemsByQueryResponse>>.Success(allSalesReturnItemsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all salesReturn ITems ", ex);
            }
        }

    
    }
}
