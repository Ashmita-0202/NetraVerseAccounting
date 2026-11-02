using FluentValidation;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignSubModulesToRole
{
    public class AssignSubModulesToRoleCommandHandler : IRequestHandler<AssignSubModulesToRoleCommand, Result<AssignSubModulesToRoleResponse>>
    {
        private readonly ISubModules _submodules;
        private readonly IValidator<AssignSubModulesToRoleCommand> _validator;

        public AssignSubModulesToRoleCommandHandler(ISubModules subModules, IValidator<AssignSubModulesToRoleCommand> validator)
        {
            _submodules = subModules;
            _validator = validator;
            
        }
        public async Task<Result<AssignSubModulesToRoleResponse>> Handle(AssignSubModulesToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(request);
                if(!validatorResult.IsValid)
                {
                    var errors = string.Join(", ", validatorResult.Errors.Select(x=>x.ErrorMessage));
                }

                var assignSubModules = await _submodules.AssignSubModulesToRole(request);

                if(assignSubModules.Errors.Any())
                {
                    var errors = string.Join(", ", assignSubModules.Errors);
                    return Result<AssignSubModulesToRoleResponse>.Failure(errors);
                }

                if(assignSubModules is null || !assignSubModules.IsSuccess)
                {
                    return Result<AssignSubModulesToRoleResponse>.Failure("Assign subModules to roles failed");
                }

                var assignSubModulesDisplay = new AssignSubModulesToRoleResponse(
                    roleId: request.roleId,
                    submodulesId: request.submodulesId.Select(x=>x.ToString()).ToList(),
                    isActive: request.isActive,
                    isAssigned: true
                    );

                return Result<AssignSubModulesToRoleResponse>.Success(assignSubModulesDisplay);

            }catch (Exception ex)
            {
                throw new Exception($"An error occurred while handling subModules to roles using {request.roleId}", ex);
            }

        }
    }
}
