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

namespace TN.Inventory.Application.Inventory.Command.UpdateItemGroup
{
public class UpdateItemGroupCommandHandler:IRequestHandler<UpdateItemGroupCommand,Result<UpdateItemGroupResponse>>
    {
        private readonly IItemGroupServices _itemGroupServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateItemGroupCommand> _validator;

        public UpdateItemGroupCommandHandler(IItemGroupServices itemGroupServices,IMapper mapper, IValidator<UpdateItemGroupCommand> validator)
        { 
            _itemGroupServices= itemGroupServices;
            _mapper= mapper;
            _validator= validator;
        }

        public async Task<Result<UpdateItemGroupResponse>> Handle(UpdateItemGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateItemGroupResponse>.Failure(errors);

                }

                var updateItemGroup = await _itemGroupServices.UpdateItemGroup(request.id, request);

                if (updateItemGroup.Errors.Any())
                {
                    var errors = string.Join(", ", updateItemGroup.Errors);
                    return Result<UpdateItemGroupResponse>.Failure(errors);
                }

                if (updateItemGroup is null || !updateItemGroup.IsSuccess)
                {
                    return Result<UpdateItemGroupResponse>.Failure("Updates units failed");
                }

                var unitsDisplay = _mapper.Map<UpdateItemGroupResponse>(updateItemGroup.Data);
                return Result<UpdateItemGroupResponse>.Success(unitsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating item group by {request.id}", ex);
            }
        }
    }
}
