using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.AddStockTransferDetails
{
    public class AddStockTransferCommandHandler : IRequestHandler<AddStockTransferCommand, Result<AddStockTransferDetailsResponse>>
    {
        private readonly IStockTransferDetailsServices _stockTransferDetailsServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddStockTransferCommand> _validator;

        public AddStockTransferCommandHandler(IStockTransferDetailsServices stockTransferDetailsServices, IMapper mapper, IValidator<AddStockTransferCommand> validator)
        {
            _stockTransferDetailsServices = stockTransferDetailsServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddStockTransferDetailsResponse>> Handle(AddStockTransferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddStockTransferDetailsResponse>.Failure(errors);
                }

                var addStockTransferDetails = await _stockTransferDetailsServices.Add(request);

                if (addStockTransferDetails.Errors.Any())
                {
                    var errors = string.Join(", ", addStockTransferDetails.Errors);
                    return Result<AddStockTransferDetailsResponse>.Failure(errors);
                }

                if (addStockTransferDetails is null || !addStockTransferDetails.IsSuccess)
                {
                    return Result<AddStockTransferDetailsResponse>.Failure(" ");
                }

                var addStockTransferDetailsDisplay = _mapper.Map<AddStockTransferDetailsResponse>(addStockTransferDetails.Data);
                return Result<AddStockTransferDetailsResponse>.Success(addStockTransferDetailsDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Stock transferDetails", ex);


            }
        }
    }
}
