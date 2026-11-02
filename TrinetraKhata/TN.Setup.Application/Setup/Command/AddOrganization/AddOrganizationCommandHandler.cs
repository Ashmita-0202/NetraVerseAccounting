using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AddModule;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddOrganization
{
    public  class AddOrganizationCommandHandler : IRequestHandler<AddOrganizationCommand,Result<AddOrganizationResponse>>
    {
        private readonly IValidator<AddOrganizationCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IOrganizationServices _oranizationServices;

        public AddOrganizationCommandHandler(IValidator<AddOrganizationCommand> validator,IMapper mapper,IOrganizationServices organizationServices) 
        {
        _validator=validator;
            _mapper=mapper;
            _oranizationServices=organizationServices;
        
        }

        public async Task<Result<AddOrganizationResponse>> Handle(AddOrganizationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return Result<AddOrganizationResponse>.Failure(errors);
                }

                var addOrganization = await _oranizationServices.Add(request);
                if (addOrganization.Errors.Any())
                {
                    var errors = string.Join(", ", addOrganization.Errors);
                    return Result<AddOrganizationResponse>.Failure(errors);

                }

                if (addOrganization is null || !addOrganization.IsSuccess)
                {
                    return Result<AddOrganizationResponse>.Failure("Add organization Failed");
                }

                var organizationDisplays = _mapper.Map<AddOrganizationResponse>(request);
                return Result<AddOrganizationResponse>.Success(organizationDisplays);

            }
            catch (Exception ex)
            {
                throw new Exception("",ex);

            }

        }
    }
}
