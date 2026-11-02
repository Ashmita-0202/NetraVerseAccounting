using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AddUser;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles
{
    public class AddPermissionToRolesCommandHandler : IRequestHandler<AddPermissionToRolesCommand, Result<AddPermissionToRolesResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPermissionToRolesCommand> _validator;

        public AddPermissionToRolesCommandHandler(IUserServices userServices, IMapper mapper, IValidator<AddPermissionToRolesCommand> validator)
        {
            _userServices = userServices;
            _mapper = mapper;
            _validator = validator;

        }
        public async Task<Result<AddPermissionToRolesResponse>> Handle(AddPermissionToRolesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<AddPermissionToRolesResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<AddPermissionToRolesResponse>.Failure("Invalid request");
                }

                var userData = await _userServices.AddPermissionToRoles(request);

                if (userData.Errors.Any())
                {
                    var errors = string.Join(", ", userData.Errors);
                    return Result<AddPermissionToRolesResponse>.Failure(errors);
                }

                if (userData is null || !userData.IsSuccess)
                {
                    return Result<AddPermissionToRolesResponse>.Failure("Assign Permission Failed");
                }
                var userDisplay = _mapper.Map<AddPermissionToRolesResponse>(request);

                if (userDisplay is null)
                {
                    return Result<AddPermissionToRolesResponse>.Failure("Mapping to AddPermission To Role Failed");
                }
                return Result<AddPermissionToRolesResponse>.Success(userDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during Assigning Permission to Roles");
            }
        }
    }
}
