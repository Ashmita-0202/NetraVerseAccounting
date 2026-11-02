using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.AddItems
{
    public class AddItemCommandHandler:IRequestHandler<AddItemCommand,Result<AddItemResponse>>
    {
        private readonly IItemsServices _itemsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddItemCommand> _validator;

        public AddItemCommandHandler(IItemsServices itemsServices,IMapper mapper,IValidator<AddItemCommand> validator ) 
        { 
             _itemsServices=itemsServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<AddItemResponse>> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddItemResponse>.Failure(errors);
                }

                var addItem = await _itemsServices.AddItem(request);

                if (addItem.Errors.Any())
                {
                    var errors = string.Join(", ", addItem.Errors);
                    return Result<AddItemResponse>.Failure(errors);
                }

                if (addItem is null || !addItem.IsSuccess)
                {
                    return Result<AddItemResponse>.Failure(" ");
                }

                var itemDisplays = _mapper.Map<AddItemResponse>(addItem.Data);
                return Result<AddItemResponse>.Success(itemDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Item", ex);


            }
        }
    }
}
