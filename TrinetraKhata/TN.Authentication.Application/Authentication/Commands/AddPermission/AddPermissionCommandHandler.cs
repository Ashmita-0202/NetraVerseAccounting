using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddPermission
{
    public class AddPermissionCommandHandler : IRequestHandler<AddPermissionCommand, Result<AddPermissionResponse>>
    {

        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<AddPermissionCommand> _validator;

        public AddPermissionCommandHandler(IUserServices userServices, IMapper mapper, IValidator<AddPermissionCommand> validator)
        {
            _userServices = userServices;
            _mapper = mapper;
            _validator = validator;

        }


        public async Task<Result<AddPermissionResponse>> Handle(AddPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return Result<AddPermissionResponse>.Failure(errors);
                }

                if (request is null)
                {
                    return Result<AddPermissionResponse>.Failure("Invalid request");
                }

                var permissionData = await _userServices.AddPermission(request);

                if (permissionData.Errors.Any())
                {
                    var errors = string.Join(", ", permissionData.Errors);
                    return Result<AddPermissionResponse>.Failure(errors);
                }

                if (permissionData is null || !permissionData.IsSuccess)
                {
                    return Result<AddPermissionResponse>.Failure("Add Permission Failed");
                }
                var permissionDisplay = _mapper.Map<AddPermissionResponse>(permissionData.Data);

                if (permissionDisplay is null)
                {
                    return Result<AddPermissionResponse>.Failure("Mapping to AddPermission Failed");
                }
                return Result<AddPermissionResponse>.Success(permissionDisplay);

            }
            catch (Exception ex)
            {
                throw new Exception("Something went wrong during Adding Permission");
            }
        }
    }
}
