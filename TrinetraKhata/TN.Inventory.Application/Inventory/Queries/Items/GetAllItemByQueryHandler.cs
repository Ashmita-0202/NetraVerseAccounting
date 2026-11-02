using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.Items
{
  public class GetAllItemByQueryHandler: IRequestHandler<GetAllItemByQuery, Result<PagedResult<GetAllItemByQueryResponse>>>
    {
        private readonly IItemsServices _itemServices;
        private readonly IMapper _mapper;

        public GetAllItemByQueryHandler(IItemsServices itemsServices, IMapper mapper)
        {
            _itemServices = itemsServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllItemByQueryResponse>>> Handle(GetAllItemByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allItem = await _itemServices.GetAllItems(request.PaginationRequest, cancellationToken);
                var allItemDisplay = _mapper.Map<PagedResult<GetAllItemByQueryResponse>>(allItem.Data);
                return Result<PagedResult<GetAllItemByQueryResponse>>.Success(allItemDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all items", ex);
            }
        }
    }
}
