using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany
{
    public class UpdateInventoryMethodCommandHandler:IRequestHandler<UpdateInventoryMethodCommand, Result<UpdateInventoryMethodResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateInventoryMethodCommand> _validator;

        public UpdateInventoryMethodCommandHandler(ISettingServices settingServices,IMapper mapper, IValidator<UpdateInventoryMethodCommand> validator) 
        {
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateInventoryMethodResponse>> Handle(UpdateInventoryMethodCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateInventoryMethodResponse>.Failure(errors);
                }

                var inventory = await _settingServices.UpdateInventoryMethodByCompany(request.companyId, request.inventoryMethod, cancellationToken);

                if (inventory.Errors.Any())
                {
                    var errors = string.Join(", ", inventory.Errors);
                    return Result<UpdateInventoryMethodResponse>.Failure(errors);
                }

                if (inventory is null || !inventory.IsSuccess)
                {
                    return Result<UpdateInventoryMethodResponse>.Failure(" ");
                }

                var inventoryDisplays = _mapper.Map<UpdateInventoryMethodResponse>(request);
                return Result<UpdateInventoryMethodResponse>.Success(inventoryDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating InventoryMethod", ex);


            }
        }
    }
}
