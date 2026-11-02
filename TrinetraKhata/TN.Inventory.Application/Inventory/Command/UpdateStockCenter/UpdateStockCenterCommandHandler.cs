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

namespace TN.Inventory.Application.Inventory.Command.UpdateStockCenter
{
    public  class UpdateStockCenterCommandHandler:IRequestHandler<UpdateStockCenterCommand,Result<UpdateStockCenterResponse>>
    {
        private readonly IStockCenterService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateStockCenterCommand> _validator;

        public UpdateStockCenterCommandHandler(IStockCenterService service,IMapper mapper,IValidator<UpdateStockCenterCommand> validator)
        {
            _service=service;
            _mapper= mapper;
            _validator= validator;
        }

        public async Task<Result<UpdateStockCenterResponse>> Handle(UpdateStockCenterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateStockCenterResponse>.Failure(errors);

                }

                var updateStockCenter = await _service.Update(request.id, request);

                if (updateStockCenter.Errors.Any())
                {
                    var errors = string.Join(", ", updateStockCenter.Errors);
                    return Result<UpdateStockCenterResponse>.Failure(errors);
                }

                if (updateStockCenter is null || !updateStockCenter.IsSuccess)
                {
                    return Result<UpdateStockCenterResponse>.Failure("Updates StockCenter failed");
                }

                var updateStockCenterDisplay = _mapper.Map<UpdateStockCenterResponse>(updateStockCenter.Data);
                return Result<UpdateStockCenterResponse>.Success(updateStockCenterDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating stock Center  by {request.id}", ex);
            }
        }
    }
}
