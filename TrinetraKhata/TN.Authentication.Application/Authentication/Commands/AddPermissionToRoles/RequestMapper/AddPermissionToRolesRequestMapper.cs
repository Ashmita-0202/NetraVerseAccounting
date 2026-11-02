using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles.RequestMapper
{
    public static class AddPermissionToRolesRequestMapper
    {
        public static AddPermissionToRolesCommand ToCommand(this AddPermissionToRolesRequest request)
        {
            return new AddPermissionToRolesCommand
                (
                request.permissionId,
                request.rolesId
                );
        }
    }
}
