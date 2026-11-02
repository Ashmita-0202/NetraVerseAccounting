using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Authentication.Application.Authentication.Commands.UpdateRoles;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdatePermission
{
   public class UpdatePermissionCommandHandler:IRequestHandler<UpdatePermissionCommand, Result<UpdatePermissionResponse>>
   {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdatePermissionCommand> _validator;

        public UpdatePermissionCommandHandler(IUserServices userServices,IMapper mapper, IValidator<UpdatePermissionCommand> validator)
        {
            _userServices=userServices;
            _mapper=mapper;
            _validator=validator;
        }

        public async Task<Result<UpdatePermissionResponse>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdatePermissionResponse>.Failure(errors);

                }

                var updatePermission = await _userServices.Update(request.id, request);

                if (updatePermission.Errors.Any())
                {
                    var errors = string.Join(", ", updatePermission.Errors);
                    return Result<UpdatePermissionResponse>.Failure(errors);
                }

                if (updatePermission is null || !updatePermission.IsSuccess)
                {
                    return Result<UpdatePermissionResponse>.Failure("Updates Permission failed");
                }

                var roleDisplay = _mapper.Map<UpdatePermissionResponse>(updatePermission.Data);
                return Result<UpdatePermissionResponse>.Success(roleDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating Permission by {request.id}", ex);
            }
        }
    }
}
