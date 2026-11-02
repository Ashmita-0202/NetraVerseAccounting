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

namespace TN.Inventory.Application.Inventory.Command.UpdateStockTransferDetails
{
    public  class UpdateStockTransferDetailsCommandHandler:IRequestHandler<UpdateStockTransferDetailsCommand,Result<UpdateStockTransferDetailsResponse>>
    {
        private readonly IStockTransferDetailsServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateStockTransferDetailsCommand> _validator;

        public UpdateStockTransferDetailsCommandHandler(IStockTransferDetailsServices services,IMapper mapper,IValidator<UpdateStockTransferDetailsCommand> validator)
        {
            _services= services;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateStockTransferDetailsResponse>> Handle(UpdateStockTransferDetailsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateStockTransferDetailsResponse>.Failure(errors);

                }

                var updateStockTransferDetails = await _services.Update(request.id, request);

                if (updateStockTransferDetails.Errors.Any())
                {
                    var errors = string.Join(", ", updateStockTransferDetails.Errors);
                    return Result<UpdateStockTransferDetailsResponse>.Failure(errors);
                }

                if (updateStockTransferDetails is null || !updateStockTransferDetails.IsSuccess)
                {
                    return Result<UpdateStockTransferDetailsResponse>.Failure("Updates Stock TransferDetails failed");
                }

                var updateDisplay = _mapper.Map<UpdateStockTransferDetailsResponse>(updateStockTransferDetails.Data);
                return Result<UpdateStockTransferDetailsResponse>.Success(updateDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating stock Transfer Details  by {request.id}", ex);
            }
        }
    }
}
