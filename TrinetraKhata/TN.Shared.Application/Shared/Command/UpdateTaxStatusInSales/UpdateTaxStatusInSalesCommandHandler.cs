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

namespace TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales
{
    public class UpdateTaxStatusInSalesCommandHandler:IRequestHandler<UpdateTaxStatusInSalesCommand, Result<UpdateTaxStatusInSalesResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateTaxStatusInSalesCommand> _validator;

        public UpdateTaxStatusInSalesCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateTaxStatusInSalesCommand> validator)
        {
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateTaxStatusInSalesResponse>> Handle(UpdateTaxStatusInSalesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateTaxStatusInSalesResponse>.Failure(errors);
                }

                var taxStatus = await _settingServices.UpdateTaxStatusInSalesBy(request.companyId, request.showTaxInSales, cancellationToken);

                if (taxStatus.Errors.Any())
                {
                    var errors = string.Join(", ", taxStatus.Errors);
                    return Result<UpdateTaxStatusInSalesResponse>.Failure(errors);
                }

                if (taxStatus is null || !taxStatus.IsSuccess)
                {
                    return Result<UpdateTaxStatusInSalesResponse>.Failure(" ");
                }

                var taxStatusDisplay = _mapper.Map<UpdateTaxStatusInSalesResponse>(request);
                return Result<UpdateTaxStatusInSalesResponse>.Success(taxStatusDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating taxStatus in Sales", ex);
            }
        }
    }
}
