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

namespace TN.Inventory.Application.Inventory.Command.AddItemGroup
{
    public class AddItemGroupCommandHandler : IRequestHandler<AddItemGroupCommand, Result<AddItemGroupResponse>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddItemGroupCommand> _validator;

        public AddItemGroupCommandHandler(IItemGroupServices itemGroupServices,IMapper mapper, IValidator<AddItemGroupCommand> validator) 
        {
            _itemGroupServices=itemGroupServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<AddItemGroupResponse>> Handle(AddItemGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddItemGroupResponse>.Failure(errors);
                }

                var addItemGroup = await _itemGroupServices.AddItemGroup(request);

                if (addItemGroup.Errors.Any())
                {
                    var errors = string.Join(", ", addItemGroup.Errors);
                    return Result<AddItemGroupResponse>.Failure(errors);
                }

                if (addItemGroup is null || !addItemGroup.IsSuccess)
                {
                    return Result<AddItemGroupResponse>.Failure(" ");
                }

                var itemGroupDisplays = _mapper.Map<AddItemGroupResponse>(request);
                return Result<AddItemGroupResponse>.Success(itemGroupDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding ItemsGroup", ex);


            }
        }
    }
}
