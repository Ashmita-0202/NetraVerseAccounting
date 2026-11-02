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

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase
{
    public  class UpdateTaxStatusInPurchaseCommandHandler:IRequestHandler<UpdateTaxStatusInPurchaseCommand, Result<UpdateTaxStatusInPurchaseResponse>>
    {
        private readonly ISettingServices _services;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateTaxStatusInPurchaseCommand> _validator;

        public UpdateTaxStatusInPurchaseCommandHandler(ISettingServices services,IMapper mapper,IValidator<UpdateTaxStatusInPurchaseCommand> validator)
        {
             _services=services;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateTaxStatusInPurchaseResponse>> Handle(UpdateTaxStatusInPurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateTaxStatusInPurchaseResponse>.Failure(errors);
                }

                var taxStatus = await _services.UpdateTaxStatusInPurchaseBy(request.companyId, request.showTaxInPurchase, cancellationToken);

                if (taxStatus.Errors.Any())
                {
                    var errors = string.Join(", ", taxStatus.Errors);
                    return Result<UpdateTaxStatusInPurchaseResponse>.Failure(errors);
                }

                if (taxStatus is null || !taxStatus.IsSuccess)
                {
                    return Result<UpdateTaxStatusInPurchaseResponse>.Failure(" ");
                }

                var taxStatusDisplay = _mapper.Map<UpdateTaxStatusInPurchaseResponse>(request);
                return Result<UpdateTaxStatusInPurchaseResponse>.Success(taxStatusDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating taxStatus in Purchase", ex);
            }
        }
    }
}
