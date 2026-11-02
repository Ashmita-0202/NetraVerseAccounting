using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Inventory.Application.Inventory.Command.UpdateItemGroup;
using TN.Inventory.Application.ServiceInterface;
using TN.Inventory.Domain.Entities;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.UpdateStockAdjustment
{
    public class UpdateStockAdjustmentCommandHandler: IRequestHandler<UpdateStockAdjustmentCommand, Result<UpdateStockAdjustmentResponse>>
    {
        private readonly IInventoriesServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateStockAdjustmentCommand> _validator;

        public UpdateStockAdjustmentCommandHandler(IInventoriesServices services,IMapper mapper,IValidator<UpdateStockAdjustmentCommand> validator)
        {
            _services = services;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateStockAdjustmentResponse>> Handle(UpdateStockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateStockAdjustmentResponse>.Failure(errors);

                }

                var updateStockAdjustment = await _services.UpdateStockAdjustment(request.id, request);

                if (updateStockAdjustment.Errors.Any())
                {
                    var errors = string.Join(", ", updateStockAdjustment.Errors);
                    return Result<UpdateStockAdjustmentResponse>.Failure(errors);
                }

                if (updateStockAdjustment is null || !updateStockAdjustment.IsSuccess)
                {
                    return Result<UpdateStockAdjustmentResponse>.Failure("Updates Stock Adjustment failed");
                }

                var unitsDisplay = _mapper.Map<UpdateStockAdjustmentResponse>(updateStockAdjustment.Data);
                return Result<UpdateStockAdjustmentResponse>.Success(unitsDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating stock adjustment by {request.id}", ex);
            }
        }
    }
}
