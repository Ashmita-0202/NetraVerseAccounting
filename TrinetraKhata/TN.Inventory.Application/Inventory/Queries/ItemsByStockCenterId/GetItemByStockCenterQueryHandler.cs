using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.Items;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.ItemsByStockCenterId
{
    public class GetItemByStockCenterQueryHandler : IRequestHandler<GetItemByStockCenterQuery, Result<PagedResult<GetItemByStockCenterQueryResponse>>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;

        public GetItemByStockCenterQueryHandler(IItemsServices itemsServices, IMapper mapper)
        {
            _itemsServices = itemsServices;
            _mapper = mapper;

        }
        public async Task<Result<PagedResult<GetItemByStockCenterQueryResponse>>> Handle(GetItemByStockCenterQuery request, CancellationToken cancellationToken)
        {
            var itemByStockCenter = await _itemsServices.GetItemByStockCenter(request.stockCenterId,request.PaginationRequest, cancellationToken);
            var itemByStockCenterDisplay = _mapper.Map<PagedResult<GetItemByStockCenterQueryResponse>>(itemByStockCenter.Data);
            return Result<PagedResult<GetItemByStockCenterQueryResponse>>.Success(itemByStockCenterDisplay);
        }
    }
}
