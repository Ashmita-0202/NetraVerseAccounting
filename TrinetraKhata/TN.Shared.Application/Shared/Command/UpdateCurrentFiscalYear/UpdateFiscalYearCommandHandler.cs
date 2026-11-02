using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear
{
    public  class UpdateFiscalYearCommandHandler:IRequestHandler<UpdateFiscalYearCommand, Result<UpdateFiscalYearResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateFiscalYearCommand> _validator;

        public UpdateFiscalYearCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateFiscalYearCommand> validator)
        {
            _settingServices = settingServices;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<UpdateFiscalYearResponse>> Handle(UpdateFiscalYearCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateFiscalYearResponse>.Failure(errors);
                }

                var fiscalYear = await _settingServices.UpdateFiscalYear(request.companyId,request.currentFiscalYearId,cancellationToken);

                if (fiscalYear.Errors.Any())
                {
                    var errors = string.Join(", ", fiscalYear.Errors);
                    return Result<UpdateFiscalYearResponse>.Failure(errors);
                }

                if (fiscalYear is null || !fiscalYear.IsSuccess)
                {
                    return Result<UpdateFiscalYearResponse>.Failure(" ");
                }

                var fiscalYearDisplay = _mapper.Map<UpdateFiscalYearResponse>(request);
                return Result<UpdateFiscalYearResponse>.Success(fiscalYearDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating Fiscal Year", ex);


            }
        }
    }
}
