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

namespace TN.Inventory.Application.Inventory.Command.AddStockAdjustment
{
    public class AddStockAdjustmentCommandHandler : IRequestHandler<AddStockAdjustmentCommand, Result<AddStockAdjustmentResponse>>
    {
        private readonly IInventoriesServices _service;
        private readonly IMapper _mapper;
        private readonly IValidator<AddStockAdjustmentCommand> _validator;

        public AddStockAdjustmentCommandHandler(IInventoriesServices services, IMapper mapper, IValidator<AddStockAdjustmentCommand> validator)
        {
            _service = services;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<AddStockAdjustmentResponse>> Handle(AddStockAdjustmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
              
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddStockAdjustmentResponse>.Failure(errors);
                }

                var addStockAdjustmentResult = await _service.AddStockAdjustment(request);

                if (addStockAdjustmentResult.Errors != null && addStockAdjustmentResult.Errors.Any())
                {
                    var errors = string.Join(", ", addStockAdjustmentResult.Errors);
                    return Result<AddStockAdjustmentResponse>.Failure(errors);
                }

            
                if (addStockAdjustmentResult == null || !addStockAdjustmentResult.IsSuccess)
                {
                    return Result<AddStockAdjustmentResponse>.Failure("Adding Stock Adjustment failed");
                }

              
                var stockAdjustmentResponse = _mapper.Map<AddStockAdjustmentResponse>(addStockAdjustmentResult.Data);

                
                return Result<AddStockAdjustmentResponse>.Success(stockAdjustmentResponse);
            }
            catch (Exception ex)
            {
             
                throw new Exception($"An error occurred while adding stock adjustment: {ex.Message}", ex);
            }
        }
    }
}
