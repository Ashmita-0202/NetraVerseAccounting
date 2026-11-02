using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Queries.ItemsById
{
   public class GetItemByIdQueryHandler: IRequestHandler<GetItemByIdQuery, Result<GetItemByIdResponse>>
    {
        private readonly IItemsServices _itemServices;
        private readonly IMapper _mapper;

        public GetItemByIdQueryHandler(IItemsServices itemsServices,IMapper mapper) 
        { 
            _itemServices=itemsServices;
            _mapper=mapper;
       
        }

        public async Task<Result<GetItemByIdResponse>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                var itemById = await _itemServices.GetItemById(request.id);
                return Result<GetItemByIdResponse>.Success(itemById.Data);

            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while fetching Items by using id", ex);
            }
        }
    }
}
