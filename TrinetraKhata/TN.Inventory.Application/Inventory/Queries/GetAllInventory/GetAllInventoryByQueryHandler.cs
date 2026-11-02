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

namespace TN.Inventory.Application.Inventory.Queries.GetAllInventory
{
    public class GetAllInventoryByQueryHandler:IRequestHandler<GetAllInventoryByQuery,Result<PagedResult<GetAllInventoryByQueryResponse>>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public GetAllInventoryByQueryHandler(IInventoriesServices inventoriesServices,IMapper mapper)
        {
            _inventoriesServices=inventoriesServices;
            _mapper=mapper;
        }

        public async Task<Result<PagedResult<GetAllInventoryByQueryResponse>>> Handle(GetAllInventoryByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allInventory = await _inventoriesServices.GetAllInventory(request.PaginationRequest, cancellationToken);
                var allInventoryDisplay = _mapper.Map<PagedResult<GetAllInventoryByQueryResponse>>(allInventory.Data);
                return Result<PagedResult<GetAllInventoryByQueryResponse>>.Success(allInventoryDisplay);


            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all inventory ", ex);
            }
        }
    }
}
