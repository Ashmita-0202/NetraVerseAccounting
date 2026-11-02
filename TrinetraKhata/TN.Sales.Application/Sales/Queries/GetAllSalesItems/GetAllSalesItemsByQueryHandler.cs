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

namespace TN.Sales.Application.Sales.Queries.GetAllSalesItems
{
    public  class GetAllSalesItemsByQueryHandler: IRequestHandler<GetAllSalesItemsByQuery, Result<PagedResult<GetAllSalesItemsByQueryResponse>>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public GetAllSalesItemsByQueryHandler(ISalesDetailsServices salesDetailsServices,IMapper mapper)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllSalesItemsByQueryResponse>>> Handle(GetAllSalesItemsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allSalesItems = await _salesDetailsServices.GetAllSalesItems(request.PaginationRequest, cancellationToken);
                var allsalesItemsDisplay = _mapper.Map<PagedResult<GetAllSalesItemsByQueryResponse>>(allSalesItems.Data);

                return Result<PagedResult<GetAllSalesItemsByQueryResponse>>.Success(allsalesItemsDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("An error while fetching all sales Items ", ex);
            }
        }
    }
}
