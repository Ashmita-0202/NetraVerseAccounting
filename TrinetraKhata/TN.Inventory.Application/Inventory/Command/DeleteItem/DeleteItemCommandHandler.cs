using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteItem
{
    public class DeleteItemCommandHandler:IRequestHandler<DeleteItemCommand, Result<bool>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;

        public DeleteItemCommandHandler(IItemsServices itemsServices,IMapper mapper)
        { 
          _itemsServices=itemsServices;
            _mapper=mapper;
        }

        public async Task<Result<bool>> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteItem = await _itemsServices.Delete(request.id, cancellationToken);
                if (deleteItem is null)
                {
                    return Result<bool>.Failure("NotFound", "items not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
