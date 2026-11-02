using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.DeleteItemGroup
{
   public class DeleteItemGroupCommandHandler:IRequestHandler<DeleteItemGroupCommand, Result<bool>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;

        public DeleteItemGroupCommandHandler(IItemGroupServices itemGroupServices, IMapper mapper) 
        {
            _itemGroupServices= itemGroupServices;
            _mapper= mapper;
        }

        public async Task<Result<bool>> Handle(DeleteItemGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteUnits = await _itemGroupServices.Delete(request.id, cancellationToken);
                if (deleteUnits is null)
                {
                    return Result<bool>.Failure("NotFound", "item group not Found");
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
