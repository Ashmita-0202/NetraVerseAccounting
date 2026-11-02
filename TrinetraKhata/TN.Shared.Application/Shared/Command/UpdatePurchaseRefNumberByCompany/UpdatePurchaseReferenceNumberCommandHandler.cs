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

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberByCompany
{
    public class UpdatePurchaseReferenceNumberCommandHandler:IRequestHandler<UpdatePurchaseReferenceNumberCommand, Result<UpdatePurchaseReferenceNumberResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePurchaseReferenceNumberCommand> _validator;

        public UpdatePurchaseReferenceNumberCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdatePurchaseReferenceNumberCommand> validator)
        {   
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<UpdatePurchaseReferenceNumberResponse>> Handle(UpdatePurchaseReferenceNumberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePurchaseReferenceNumberResponse>.Failure(errors);
                }

                var purchaseRef = await _settingServices.UpdatePurchaseReferenceNumberByCompany(request.companyId, request.PurchaseReferencesType, cancellationToken);

                if (purchaseRef.Errors.Any())
                {
                    var errors = string.Join(", ", purchaseRef.Errors);
                    return Result<UpdatePurchaseReferenceNumberResponse>.Failure(errors);
                }

                if (purchaseRef is null || !purchaseRef.IsSuccess)
                {
                    return Result<UpdatePurchaseReferenceNumberResponse>.Failure(" ");
                }

                var purchaseRefDisplay = _mapper.Map<UpdatePurchaseReferenceNumberResponse>(request);
                return Result<UpdatePurchaseReferenceNumberResponse>.Success(purchaseRefDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating purchase reference number", ex);
            }
        }
    }
}
