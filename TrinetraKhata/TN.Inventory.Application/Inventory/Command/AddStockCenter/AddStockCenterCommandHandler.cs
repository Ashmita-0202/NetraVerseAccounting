using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Inventory.Application.Inventory.Command.AddItems;
using TN.Inventory.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Inventory.Application.Inventory.Command.AddStockCenter
{
    public class AddStockCenterCommandHandler: IRequestHandler<AddStockCenterCommand, Result<AddStockCenterResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IStockCenterService _service;
        private readonly IValidator<AddStockCenterCommand> _validator;

        public AddStockCenterCommandHandler(IStockCenterService service,IMapper mapper,IValidator<AddStockCenterCommand> validator)
        {
            _mapper = mapper;
            _service = service;
            _validator = validator;
        }

        public async Task<Result<AddStockCenterResponse>> Handle(AddStockCenterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddStockCenterResponse>.Failure(errors);
                }

                var addStockCenter = await _service.Add(request);

                if (addStockCenter.Errors.Any())
                {
                    var errors = string.Join(", ", addStockCenter.Errors);
                    return Result<AddStockCenterResponse>.Failure(errors);
                }

                if (addStockCenter is null || !addStockCenter.IsSuccess)
                {
                    return Result<AddStockCenterResponse>.Failure(" ");
                }

                var addStockCenterDisplay = _mapper.Map<AddStockCenterResponse>(addStockCenter.Data);
                return Result<AddStockCenterResponse>.Success(addStockCenterDisplay);


            }
            catch (Exception ex)
            {
                throw;
                //throw new Exception("An error occurred while adding StockCenter", ex);
            }
        }
    }
}
