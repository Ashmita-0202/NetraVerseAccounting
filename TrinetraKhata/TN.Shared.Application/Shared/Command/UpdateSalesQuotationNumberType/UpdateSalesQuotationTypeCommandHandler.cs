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

namespace TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType
{
    public  class UpdateSalesQuotationTypeCommandHandler: IRequestHandler<UpdateSalesQuotationTypeCommand, Result<UpdateSalesQuotationTypeResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSalesQuotationTypeCommand> _validator;

        public UpdateSalesQuotationTypeCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateSalesQuotationTypeCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateSalesQuotationTypeResponse>> Handle(UpdateSalesQuotationTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSalesQuotationTypeResponse>.Failure(errors);
                }

                var quotationNumber = await _settingServices.UpdateSalesQuotationType(request.companyId, request.salesQuotationNumberType, cancellationToken);

                if (quotationNumber.Errors.Any())
                {
                    var errors = string.Join(", ", quotationNumber.Errors);
                    return Result<UpdateSalesQuotationTypeResponse>.Failure(errors);
                }

                if (quotationNumber is null || !quotationNumber.IsSuccess)
                {
                    return Result<UpdateSalesQuotationTypeResponse>.Failure(" ");
                }

                var quotationNumberDisplay = _mapper.Map<UpdateSalesQuotationTypeResponse>(request);
                return Result<UpdateSalesQuotationTypeResponse>.Success(quotationNumberDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Sales quotation number", ex);
            }
        }
    }
}
