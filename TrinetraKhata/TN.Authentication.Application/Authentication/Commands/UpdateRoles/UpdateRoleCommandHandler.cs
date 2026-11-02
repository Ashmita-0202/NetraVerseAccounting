using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdateRoles
{
  public class UpdateRoleCommandHandler:IRequestHandler<UpdateRoleCommand,Result<UpdateRoleResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateRoleCommand> _validator;

        public UpdateRoleCommandHandler(IUserServices userServices,IMapper mapper,IValidator<UpdateRoleCommand> validator)
        { 
            _userServices=userServices;
            _mapper=mapper;
            _validator=validator;
        
        }

        public async Task<Result<UpdateRoleResponse>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateRoleResponse>.Failure(errors);

                }

                var updateRole = await _userServices.Update(request.Id, request);

                if (updateRole.Errors.Any())
                {
                    var errors = string.Join(", ", updateRole.Errors);
                    return Result<UpdateRoleResponse>.Failure(errors);
                }

                if (updateRole is null || !updateRole.IsSuccess)
                {
                    return Result<UpdateRoleResponse>.Failure("Updates Role failed");
                }

                var roleDisplay = _mapper.Map<UpdateRoleResponse>(updateRole.Data);
                return Result<UpdateRoleResponse>.Success(roleDisplay);
            }
            catch (Exception ex)
            {
                throw new Exception("$\"An error occurred while updating role by {request.id}", ex);
            }
        }
    }
}
