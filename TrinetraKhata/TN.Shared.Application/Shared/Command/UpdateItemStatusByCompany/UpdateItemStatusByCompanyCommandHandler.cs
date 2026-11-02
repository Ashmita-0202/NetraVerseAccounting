using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusByCompany
{
    public class UpdateItemStatusByCompanyCommandHandler : IRequestHandler<UpdateItemStatusByCompanyCommand, Result<UpdateItemStatusByCompanyResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateItemStatusByCompanyCommand> _validator;

        public UpdateItemStatusByCompanyCommandHandler(IValidator<UpdateItemStatusByCompanyCommand> validator,ISettingServices settingServices, IMapper mapper)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;

            
        }
        public async Task<Result<UpdateItemStatusByCompanyResponse>> Handle(UpdateItemStatusByCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateItemStatusByCompanyResponse>.Failure(errors);
                }

                var expiredDateItem = await _settingServices.UpdateItemStatusByCompany(request.companyId, request.isExpiredDate,request.isSerialNo, cancellationToken);

                if (expiredDateItem.Errors.Any())
                {
                    var errors = string.Join(", ", expiredDateItem.Errors);
                    return Result<UpdateItemStatusByCompanyResponse>.Failure(errors);
                }

                if (expiredDateItem is null || !expiredDateItem.IsSuccess)
                {
                    return Result<UpdateItemStatusByCompanyResponse>.Failure(" ");
                }

                var customerDisplays = _mapper.Map<UpdateItemStatusByCompanyResponse>(request);
                return Result<UpdateItemStatusByCompanyResponse>.Success(customerDisplays);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding Customer", ex);


            }
        }
    }
}
