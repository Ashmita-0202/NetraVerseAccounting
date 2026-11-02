using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.ServiceInterface;
using TN.Setup.Application.Setup.Command.AssignModulesToRole;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignMenusToRole
{
    public class AssignMenusToRoleCommandHandler : IRequestHandler<AssignMenusToRoleCommands, Result<AssignMenuToRolesResponse>>
    {

        private readonly IMenuServices _menuServices;
        private readonly IValidator<AssignMenusToRoleCommands> _validator;

        public AssignMenusToRoleCommandHandler(IMenuServices menuServices, IValidator<AssignMenusToRoleCommands> validator)
        {
            _menuServices = menuServices;
            _validator = validator;
            
        }
        public async Task<Result<AssignMenuToRolesResponse>> Handle(AssignMenusToRoleCommands request, CancellationToken cancellationToken)
        {
            try
            {
                var validateResult = await _validator.ValidateAsync(request, cancellationToken);
                if(!validateResult.IsValid)
                {
                    var errors = string.Join(", ", validateResult.Errors.Select(x => x.ErrorMessage));

                }

                var assignMenu = await _menuServices.AssignMenuToRoles(request, cancellationToken);

                if (assignMenu.Errors.Any())
                {
                    var errors = string.Join(", ", assignMenu.Errors);
                    return Result<AssignMenuToRolesResponse>.Failure(errors);

                }

                if (assignMenu is null || !assignMenu.IsSuccess)
                {
                    return Result<AssignMenuToRolesResponse>.Failure("Assign menu to roles failed");

                }

                var assignMenuDisplay = new AssignMenuToRolesResponse(
                    roleId: request.roleId,
                    menusId: request.menusId.Select(id => id.ToString()).ToList(),
                    isActive: request.isActive,
                    isAssigned:true
                    );
                return Result<AssignMenuToRolesResponse>.Success(assignMenuDisplay);

            }
            catch(Exception ex)
            {
                throw new Exception($"An error occurred while assigning menus to {request.roleId}", ex); ;
            }
        }
    }
}
