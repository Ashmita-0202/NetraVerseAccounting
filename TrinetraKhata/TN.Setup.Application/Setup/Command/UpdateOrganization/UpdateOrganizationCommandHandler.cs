using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateModules;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateOrganization
{
    public class UpdateOrganizationCommandHandler: IRequestHandler<UpdateOrganizationCommand, Result<UpdateOrganizationResponse>>
    {
        private readonly IValidator<UpdateOrganizationCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IOrganizationServices _organizationServices;

        public UpdateOrganizationCommandHandler(IValidator<UpdateOrganizationCommand> validator,IMapper mapper,IOrganizationServices organizationServices)
        {
           _validator = validator;
            _mapper=mapper;
            _organizationServices=organizationServices;
        }

        public async Task<Result<UpdateOrganizationResponse>> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateOrganizationResponse>.Failure(errors);

                }

                var updateOrganization = await _organizationServices.Update(request.Id, request);

                if (updateOrganization.Errors.Any())
                {
                    var errors = string.Join(", ", updateOrganization.Errors);
                    return Result<UpdateOrganizationResponse>.Failure(errors);
                }

                if (updateOrganization is null || !updateOrganization.IsSuccess)
                {
                    return Result<UpdateOrganizationResponse>.Failure("Updates modules failed");
                }

                var organizationDisplay = _mapper.Map<UpdateOrganizationResponse>(updateOrganization.Data);
                return Result<UpdateOrganizationResponse>.Success(organizationDisplay);


            }
            catch (Exception ex)
            {
             throw new Exception("An error occurred while updating organization", ex);
            
            }
        }
    }
}
