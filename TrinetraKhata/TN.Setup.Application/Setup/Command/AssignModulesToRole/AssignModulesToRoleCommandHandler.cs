using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignModulesToRole
{
    public class AssignModulesToRoleCommandHandler : IRequestHandler<AssignModulesToRoleCommand, Result<AssignModulesToRoleResponse>>
    {
        private readonly IModule _moduleServices;
        private readonly IValidator<AssignModulesToRoleCommand> _validator;

        public AssignModulesToRoleCommandHandler(IValidator<AssignModulesToRoleCommand> validator, IModule moduleServices)
        {
            _moduleServices = moduleServices;
            _validator = validator;
            
        }
        public async Task<Result<AssignModulesToRoleResponse>> Handle(AssignModulesToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

                }

                var assignModules = await _moduleServices.AssignModulesToRole(request);

                if(assignModules.Errors.Any())
                {
                    var errors = string.Join(", ", assignModules.Errors);
                    return Result<AssignModulesToRoleResponse>.Failure(errors);

                }

                if(assignModules is null || !assignModules.IsSuccess)
                {
                    return Result<AssignModulesToRoleResponse>.Failure("Assign modules to roles failed");

                }

                var assignModulesDisplay = new AssignModulesToRoleResponse(
                    roleId: request.roleId,
                    moduleId: request.modulesId.Select(id => id.ToString()).ToList(),
                    isAssigned: true,
                    isActive: request.isActive
                    );
                return Result<AssignModulesToRoleResponse>.Success(assignModulesDisplay);

            }catch (Exception ex)
            {
                throw new Exception("An error occurred while handling modules to roles in AssignModulesToRoleCommandHandler", ex);
            }

        }
    }
}
