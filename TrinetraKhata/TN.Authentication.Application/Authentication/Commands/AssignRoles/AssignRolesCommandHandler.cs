using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AssignRoles
{
    public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, Result<AssignRolesResponse>>
    {
        private readonly IValidator<AssignRolesCommand> _validator;
        private readonly IUserServices _userServices;
        public AssignRolesCommandHandler(IValidator<AssignRolesCommand> validator, IUserServices userServices)
        {
            _userServices = userServices;
            _validator = validator;
        }

        public async Task<Result<AssignRolesResponse>> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                    return Result<AssignRolesResponse>.Failure(errors);
                }

                var assignRoles = await _userServices.AssignMultipleRoles(request);

                if(assignRoles.Errors.Any())
                {
                    var errors = string.Join(", ", assignRoles.Errors);
                    return Result<AssignRolesResponse>.Failure(errors);
                }
                if(assignRoles is null || !assignRoles.IsSuccess)
                {
                    return Result<AssignRolesResponse>.Failure("AssignRoles Failed");
                }

                var (userId, RoleName) = assignRoles.Data;
                return Result<AssignRolesResponse>.Success(new AssignRolesResponse(userId, RoleName));

            }catch(Exception ex)
            {
                throw new Exception("An error occurred while assigning roles");
            }
        }
    }
}
