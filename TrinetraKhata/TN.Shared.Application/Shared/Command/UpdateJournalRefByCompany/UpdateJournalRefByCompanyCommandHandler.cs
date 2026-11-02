using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Shared.Application.ServiceInterface;
using TN.Shared.Application.Shared.Command.UpdateItemStatusByCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Shared.Application.Shared.Command.UpdateJournalRefByCompany
{
    public  class UpdateJournalRefByCompanyCommandHandler:IRequestHandler<UpdateJournalRefByCompanyCommand, Result<UpdateJournalRefByCompanyResponse>>
    {
        private readonly ISettingServices _settingServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateJournalRefByCompanyCommand> _validator;

        public UpdateJournalRefByCompanyCommandHandler(ISettingServices settingServices,IMapper mapper,IValidator<UpdateJournalRefByCompanyCommand> validator)
        {
            _settingServices=settingServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdateJournalRefByCompanyResponse>> Handle(UpdateJournalRefByCompanyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateJournalRefByCompanyResponse>.Failure(errors);
                }

                var journalRef = await _settingServices.UpdateJournalRefByCompany( request.companyId, request.journalReferences, cancellationToken);

                if (journalRef.Errors.Any())
                {
                    var errors = string.Join(", ", journalRef.Errors);
                    return Result<UpdateJournalRefByCompanyResponse>.Failure(errors);
                }

                if (journalRef is null || !journalRef.IsSuccess)
                {
                    return Result<UpdateJournalRefByCompanyResponse>.Failure(" ");
                }

                var journalRefDisplay = _mapper.Map<UpdateJournalRefByCompanyResponse>(request);
                return Result<UpdateJournalRefByCompanyResponse>.Success(journalRefDisplay);


            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating journal reference number", ex);


            }
        }
    }
}
