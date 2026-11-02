using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.GetAllInventory;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Inventory.Application.Inventory.Queries.GetAllInventoryLogs
{
    public  class GetAllInventoriesLogsByQueryHandler:IRequestHandler<GetAllInventoriesLogsByQuery,Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public GetAllInventoriesLogsByQueryHandler(IInventoriesServices inventoriesServices,IMapper mapper)
        {
                 _inventoriesServices= inventoriesServices;
                 _mapper= mapper;
        }

        public async Task<Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>> Handle(GetAllInventoriesLogsByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allInventoryLogs = await _inventoriesServices.GetAllInventoriesLogs(request.PaginationRequest, cancellationToken);
                var allInventoryLogsDisplay = _mapper.Map<PagedResult<GetAllInventoriesLogsByQueryResponse>>(allInventoryLogs.Data);
                return Result<PagedResult<GetAllInventoriesLogsByQueryResponse>>.Success(allInventoryLogsDisplay);

            }
            catch (Exception ex)
            {

                throw new Exception("An error occured while fetching all InventoryLogs ", ex);
            }
        }
    }
}
