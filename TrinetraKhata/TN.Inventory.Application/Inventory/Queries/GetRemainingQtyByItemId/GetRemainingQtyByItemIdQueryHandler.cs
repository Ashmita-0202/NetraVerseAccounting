using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.Inventory.Queries.GetRemainingQuantityByItemId;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.GetRemainingQtyByItemId
{
   public class GetRemainingQtyByItemIdQueryHandler: IRequestHandler<GetRemainingQtyByItemIdQuery, Result<GetRemainingQtyByItemIdQueryResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IInventoriesServices _inventoriesServices;

        public GetRemainingQtyByItemIdQueryHandler(IInventoriesServices inventoriesServices,IMapper mapper)
        {
            _mapper = mapper;
            _inventoriesServices=inventoriesServices;
        }

        public async Task<Result<GetRemainingQtyByItemIdQueryResponse>> Handle(GetRemainingQtyByItemIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allRemainingQty = await _inventoriesServices.GetRemainingQtyByItemId(request.ItemId, cancellationToken);

                if (!allRemainingQty.IsSuccess)
                {
                    return Result<GetRemainingQtyByItemIdQueryResponse>.Failure(allRemainingQty.Message);
                }

                var allRemainingQtyDisplay = _mapper.Map<GetRemainingQtyByItemIdQueryResponse>(allRemainingQty.Data);
                return Result<GetRemainingQtyByItemIdQueryResponse>.Success(allRemainingQtyDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while fetching RemainingQty by ItemId: {request.ItemId}", ex);
            }
        }
    }
}
