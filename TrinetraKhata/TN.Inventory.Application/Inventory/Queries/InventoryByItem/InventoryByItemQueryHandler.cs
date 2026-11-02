using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.Inventory.Queries.FilterConversionFactorByDate;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.InventoryByItem
{
    public class InventoryByItemQueryHandler : IRequestHandler<InventoryByItemQuery, Result<IEnumerable<InventoryByItemQueryResponse>>>
    {
        private readonly IInventoriesServices _inventoriesServices;
        private readonly IMapper _mapper;

        public InventoryByItemQueryHandler(IInventoriesServices inventoriesServices, IMapper mapper)
        {
            _inventoriesServices = inventoriesServices;
            _mapper = mapper;
            
        }
        public async Task<Result<IEnumerable<InventoryByItemQueryResponse>>> Handle(InventoryByItemQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterInventory = await _inventoriesServices.GetInventoryItem(request.itemId, cancellationToken);

                if (!filterInventory.IsSuccess || filterInventory.Data == null)
                {
                    return Result<IEnumerable<InventoryByItemQueryResponse>>.Failure(filterInventory.Message);
                }

                var filterInventoryResult = _mapper.Map<IEnumerable<InventoryByItemQueryResponse>>(filterInventory.Data);

                return Result<IEnumerable<InventoryByItemQueryResponse>>.Success(filterInventoryResult);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting inventory by Item{request.itemId}", ex);
            }
        }
    }
}
