using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateSalesReturnType
{
    public  class UpdateSalesReturnTypeCommandHandler:IRequestHandler<UpdateSalesReturnTypeCommand, Result<UpdateSalesReturnTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSalesReturnTypeCommand> _validator;

        public UpdateSalesReturnTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateSalesReturnTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;

        }

        public async Task<Result<UpdateSalesReturnTypeResponse>> Handle(UpdateSalesReturnTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSalesReturnTypeResponse>.Failure(errors);
                }

                var returnNumber = await _settingServices.UpdateSalesReturnType(request.companyId, request.salesReturnNumberType, cancellationToken);

                if (returnNumber.Errors.Any())
                {
                    var errors = string.Join(", ", returnNumber.Errors);
                    return Result<UpdateSalesReturnTypeResponse>.Failure(errors);
                }

                if (returnNumber is null || !returnNumber.IsSuccess)
                {
                    return Result<UpdateSalesReturnTypeResponse>.Failure(" ");
                }

                var returnNumberDisplay = _mapper.Map<UpdateSalesReturnTypeResponse>(request);
                return Result<UpdateSalesReturnTypeResponse>.Success(returnNumberDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating sales return number", ex);
            }
        }
    }
}
