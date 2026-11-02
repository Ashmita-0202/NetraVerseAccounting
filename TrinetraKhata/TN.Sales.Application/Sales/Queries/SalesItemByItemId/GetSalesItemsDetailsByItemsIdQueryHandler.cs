using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Sales.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;


namespace TN.Sales.Application.Sales.Queries.SalesItemByItemId
{
    public class GetSalesItemsDetailsByItemsIdQueryHandler : IRequestHandler<GetSalesItemDetailsByItemIdQuery, Result<GetSalesItemsDetailsByItemIdQueryResponse>>
    {
        private readonly ISalesDetailsServices _salesDetailsServices;
        private readonly IMapper _mapper;

        public GetSalesItemsDetailsByItemsIdQueryHandler(ISalesDetailsServices salesDetailsServices, IMapper mapper)
        {
            _salesDetailsServices=salesDetailsServices;
            _mapper=mapper;
        } 
        public async Task<Result<GetSalesItemsDetailsByItemIdQueryResponse>> Handle(GetSalesItemDetailsByItemIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var salesDetailsItems = await _salesDetailsServices.GetSalesDetailsItems(request.itemsId);

                return Result<GetSalesItemsDetailsByItemIdQueryResponse>.Success(salesDetailsItems.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching sales Details by Id", ex);

            }
        }
    }
}
