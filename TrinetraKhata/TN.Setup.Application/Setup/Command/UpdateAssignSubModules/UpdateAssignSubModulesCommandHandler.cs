using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateAssignSubModules
{
    public class UpdateAssignSubModulesCommandHandler : IRequestHandler<UpdateAssignSubModulesCommand, Result<UpdateAssignSubModulesResponse>>
    {
        private readonly ISubModules _subModules;
        private readonly IValidator<UpdateAssignSubModulesCommand> _validator;

        public UpdateAssignSubModulesCommandHandler(ISubModules subModules, IValidator<UpdateAssignSubModulesCommand> validator)
        {
            _subModules = subModules;
            _validator = validator;
            
        }
        public async Task<Result<UpdateAssignSubModulesResponse>> Handle(UpdateAssignSubModulesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAssignSubModulesResponse>.Failure(errors);

                }

                var updateAssignModules = await _subModules.UpdateAssignSubModules(request);

                if (updateAssignModules.Errors.Any())
                {
                    var errors = string.Join(", ", updateAssignModules.Errors);
                    return Result<UpdateAssignSubModulesResponse>.Failure(errors);
                }

                if (updateAssignModules is null || !updateAssignModules.IsSuccess)
                {
                    return Result<UpdateAssignSubModulesResponse>.Failure("Updates modules failed");
                }

                var updateAssignSubModulesResult = new UpdateAssignSubModulesResponse(
                    subModulesId: request.subModulesId,
                    roleId: request.roleId,
                    isActive: request.isActive
                    );
                return Result<UpdateAssignSubModulesResponse>.Success(updateAssignSubModulesResult);

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while Updating AssignSubModules by {request.subModulesId}", ex);
            }
        }
    }
}
