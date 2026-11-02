using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.Login;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.Roles
{
    public sealed class RoleCommandHandler : IRequestHandler<RoleCommand, Result<RoleResponse>>
    {
        private readonly IUserServices _userServices;
        private readonly IValidator<RoleCommand> _validator;

        public RoleCommandHandler(IUserServices userServices, IValidator<RoleCommand> validator)
        {
            _userServices = userServices;
            _validator = validator;
            
        }
        public async Task<Result<RoleResponse>> Handle(RoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x=>x.ErrorMessage));
                    return Result<RoleResponse>.Failure(errors);

                }

                var role = await _userServices.CreateRoles(request.Name);

                if (role is null || !role.IsSuccess)
                {
                    var errors = string.Join(", ", (role?.Errors ?? new List<string>()).Select(x => x.ToString()));
                    return Result<RoleResponse>.Failure(errors);
                }

                string rolename = role.Message;
                return Result<RoleResponse>.Success(new RoleResponse(rolename));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occured while creating roles");
            }
        }
    }
}
