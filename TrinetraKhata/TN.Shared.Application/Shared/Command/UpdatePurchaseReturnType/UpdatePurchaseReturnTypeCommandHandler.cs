using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType
{
    public  class UpdatePurchaseReturnTypeCommandHandler: IRequestHandler<UpdatePurchaseReturnTypeCommand, Result<UpdatePurchaseReturnTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePurchaseReturnTypeCommand> _validator;

        public UpdatePurchaseReturnTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdatePurchaseReturnTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;



        }

        public async Task<Result<UpdatePurchaseReturnTypeResponse>> Handle(UpdatePurchaseReturnTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePurchaseReturnTypeResponse>.Failure(errors);
                }

                var returnNumber = await _settingServices.UpdatePurchaseReturnType(request.companyId, request.purchaseReturnNumberType, cancellationToken);

                if (returnNumber.Errors.Any())
                {
                    var errors = string.Join(", ", returnNumber.Errors);
                    return Result<UpdatePurchaseReturnTypeResponse>.Failure(errors);
                }

                if (returnNumber is null || !returnNumber.IsSuccess)
                {
                    return Result<UpdatePurchaseReturnTypeResponse>.Failure(" ");
                }

                var returnNumberDisplay = _mapper.Map<UpdatePurchaseReturnTypeResponse>(request);
                return Result<UpdatePurchaseReturnTypeResponse>.Success(returnNumberDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating purchase return number", ex);
            }
        }
    }
}
