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

namespace TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType
{
    public  class UpdatePurchaseQuotationTypeCommandHandler: IRequestHandler<UpdatePurchaseQuotationTypeCommand, Result<UpdatePurchaseQuotationTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePurchaseQuotationTypeCommand> _validator;

        public UpdatePurchaseQuotationTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdatePurchaseQuotationTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdatePurchaseQuotationTypeResponse>> Handle(UpdatePurchaseQuotationTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePurchaseQuotationTypeResponse>.Failure(errors);
                }

                var quotationNumber = await _settingServices.UpdatePurchaseQuotationType(request.companyId, request.purchaseQuotationNumberType, cancellationToken);

                if (quotationNumber.Errors.Any())
                {
                    var errors = string.Join(", ", quotationNumber.Errors);
                    return Result<UpdatePurchaseQuotationTypeResponse>.Failure(errors);
                }

                if (quotationNumber is null || !quotationNumber.IsSuccess)
                {
                    return Result<UpdatePurchaseQuotationTypeResponse>.Failure(" ");
                }

                var quotationNumberDisplay = _mapper.Map<UpdatePurchaseQuotationTypeResponse>(request);
                return Result<UpdatePurchaseQuotationTypeResponse>.Success(quotationNumberDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating purchase quotation number", ex);
            }
        }
    }
}
