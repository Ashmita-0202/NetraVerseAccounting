using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateOrganization;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateInstitution
{
    public class UpdateInstitutionCommandHandler: IRequestHandler<UpdateInstitutionCommand, Result<UpdateInstitutionResponse>>
    {
        private readonly IValidator<UpdateInstitutionCommand> _validator;
        private readonly IInstitutionServices _institutionServices;
        private readonly IMapper _mapper;

        public UpdateInstitutionCommandHandler(IValidator<UpdateInstitutionCommand> validator,IMapper mapper,IInstitutionServices institutionServices)
        { 
     
            _validator=validator;
            _institutionServices=institutionServices;
            _mapper=mapper;
        }

        public async Task<Result<UpdateInstitutionResponse>> Handle(UpdateInstitutionCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateInstitutionResponse>.Failure(errors);

                }

                var updateInstitution= await _institutionServices.Update(request.Id, request);

                if (updateInstitution.Errors.Any())
                {
                    var errors = string.Join(", ", updateInstitution.Errors);
                    return Result<UpdateInstitutionResponse>.Failure(errors);
                }

                if (updateInstitution is null || !updateInstitution.IsSuccess)
                {
                    return Result<UpdateInstitutionResponse>.Failure("Updates modules failed");
                }

                var organizationDisplay = _mapper.Map<UpdateInstitutionResponse>(updateInstitution.Data);
                return Result<UpdateInstitutionResponse>.Success(organizationDisplay);

            }
            catch (Exception ex)
            {

                throw new Exception(" An error occurred while updating Institution", ex);
            
            }
        }
    }
}
