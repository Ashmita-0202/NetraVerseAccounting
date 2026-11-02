using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateItem
{
    public class UpdateItemCommandHandler:IRequestHandler<UpdateItemCommand, Result<UpdateItemResponse>>
    {
        private readonly IItemsServices _itemServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateItemCommand> _validator;

        public UpdateItemCommandHandler(IItemsServices itemsServices, IMapper mapper, IValidator<UpdateItemCommand> validator) 
        { 
            _itemServices= itemsServices;
            _mapper= mapper;
            _validator= validator;

        }

        public async  Task<Result<UpdateItemResponse>> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateItemResponse>.Failure(errors);

                }

                var updateItem = await _itemServices.UpdateItem(request.id, request);

                if (updateItem.Errors.Any())
                {
                    var errors = string.Join(", ", updateItem.Errors);
                    return Result<UpdateItemResponse>.Failure(errors);
                }

                if (updateItem is null || !updateItem.IsSuccess)
                {
                    return Result<UpdateItemResponse>.Failure("Updates Item failed");
                }

                var itemDisplay = _mapper.Map<UpdateItemResponse>(updateItem.Data);
                return Result<UpdateItemResponse>.Success(itemDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating item by {request.id}", ex);
            }

        }
    }
}
