using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AssignSubModulesToRole;
using TN.Setup.Application.Setup.Command.UpdateCompany;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateAssignModules
{
    public class UpdateAssignModulesCommandHandler : IRequestHandler<UpdateAssignModulesCommand, Result<UpdateAssignModulesResponse>>
    {

        public readonly IModule _module;
        private readonly IValidator<UpdateAssignModulesCommand> _validator;

        public UpdateAssignModulesCommandHandler(IModule module, IValidator<UpdateAssignModulesCommand> validator)
        {
            _validator = validator;
            _module = module;
            
        }
        public async Task<Result<UpdateAssignModulesResponse>> Handle(UpdateAssignModulesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAssignModulesResponse>.Failure(errors);

                }

                var updateAssignModules = await _module.UpdateAssignModules(request);

                if (updateAssignModules.Errors.Any())
                {
                    var errors = string.Join(", ", updateAssignModules.Errors);
                    return Result<UpdateAssignModulesResponse>.Failure(errors);
                }

                if (updateAssignModules is null || !updateAssignModules.IsSuccess)
                {
                    return Result<UpdateAssignModulesResponse>.Failure("Updates modules failed");
                }

                var updateAssignModulesResult = new UpdateAssignModulesResponse(
                    modulesId: request.modulesId,
                    roleId: request.roleId,
                    isActive: request.isActive
                    );
                return Result<UpdateAssignModulesResponse>.Success(updateAssignModulesResult);

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while updating AssignModules by {request.modulesId} ", ex);
            }
        }
    }
}
