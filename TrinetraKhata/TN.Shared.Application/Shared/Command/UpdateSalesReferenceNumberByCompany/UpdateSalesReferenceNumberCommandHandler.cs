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

namespace TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberByCompany
{
    public  class UpdateSalesReferenceNumberCommandHandler:IRequestHandler<UpdateSalesReferenceNumberCommand,Result<UpdateSalesReferenceNumberResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateSalesReferenceNumberCommand> _validator;

        public UpdateSalesReferenceNumberCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateSalesReferenceNumberCommand> validator) 
        {
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<UpdateSalesReferenceNumberResponse>> Handle(UpdateSalesReferenceNumberCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateSalesReferenceNumberResponse>.Failure(errors);
                }

                var salesRef = await _settingServices.UpdateSalesReferenceNumberByCompany(request.companyId, request.showReferenceNumberForSales, cancellationToken);

                if (salesRef.Errors.Any())
                {
                    var errors = string.Join(", ", salesRef.Errors);
                    return Result<UpdateSalesReferenceNumberResponse>.Failure(errors);
                }

                if (salesRef is null || !salesRef.IsSuccess)
                {
                    return Result<UpdateSalesReferenceNumberResponse>.Failure(" ");
                }

                var salesRefDisplay = _mapper.Map<UpdateSalesReferenceNumberResponse>(request);
                return Result<UpdateSalesReferenceNumberResponse>.Success(salesRefDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating sales reference number", ex);


            }
        }
    }
}
