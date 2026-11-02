using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.ItemsById;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.InventoriesLogsById
{
    public  class GetInventoriesLogsByIdQueryHandler:IRequestHandler<GetInventoriesLogsByIdQuery,Result<GetInventoriesLogsByIdQueryResponse>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public GetInventoriesLogsByIdQueryHandler(IInventoriesServices inventoriesServices,IMapper mapper)
        {
            _inventoriesServices=inventoriesServices;
            _mapper=mapper;
        }

        public async Task<Result<GetInventoriesLogsByIdQueryResponse>> Handle(GetInventoriesLogsByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var inventoriesLogsById = await _inventoriesServices.GetInventoriesLogsById(request.id);
                return Result<GetInventoriesLogsByIdQueryResponse>.Success(inventoriesLogsById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching InventoriesLogs by using id", ex);
            }
        }
    }
}
