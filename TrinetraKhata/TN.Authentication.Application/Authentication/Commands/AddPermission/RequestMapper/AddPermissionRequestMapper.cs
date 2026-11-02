using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermission.RequestMapper
{
    public static class AddPermissionRequestMapper
    {
        public static AddPermissionCommand ToCommand(this AddPermissionRequest request)
        {
            return new AddPermissionCommand
                (
                    request.name,
                    request.roleId
                );
        }
    }
}
