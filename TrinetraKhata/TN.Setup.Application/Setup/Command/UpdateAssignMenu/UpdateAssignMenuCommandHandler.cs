using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.UpdateAssignModules;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateAssignMenu
{
    public class UpdateAssignMenuCommandHandler : IRequestHandler<UpdateAssignMenuCommand, Result<UpdateAssignMenuResponse>>
    {
        private readonly IMenuServices _menuServices;
        private readonly IValidator<UpdateAssignMenuCommand> _validator;

        public UpdateAssignMenuCommandHandler(IMenuServices menuServices, IValidator<UpdateAssignMenuCommand> validator)
        {
            _validator = validator;
            _menuServices = menuServices;
            
        }
        public async Task<Result<UpdateAssignMenuResponse>> Handle(UpdateAssignMenuCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationAction = await _validator.ValidateAsync(request, cancellationToken);
                if ((!validationAction.IsValid))
                {
                    var errors = string.Join(", ", validationAction.Errors.Select(x => x.ErrorMessage));
                    return Result<UpdateAssignMenuResponse>.Failure(errors);

                }

                var updateAssignMenu = await _menuServices.UpdateAssignMenu(request);

                if (updateAssignMenu.Errors.Any())
                {
                    var errors = string.Join(", ", updateAssignMenu.Errors);
                    return Result<UpdateAssignMenuResponse>.Failure(errors);
                }

                if (updateAssignMenu is null || !updateAssignMenu.IsSuccess)
                {
                    return Result<UpdateAssignMenuResponse>.Failure("Updates modules failed");
                }

                var updateAssignMenuResult = new UpdateAssignMenuResponse(
                    menuId: request.menuId,
                    roleId: request.roleId,
                    isActive: request.isActive
                    );
                return Result<UpdateAssignMenuResponse>.Success(updateAssignMenuResult);

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating AssignModules by {request.menuId} ", ex);
            }
        }
    }
}
