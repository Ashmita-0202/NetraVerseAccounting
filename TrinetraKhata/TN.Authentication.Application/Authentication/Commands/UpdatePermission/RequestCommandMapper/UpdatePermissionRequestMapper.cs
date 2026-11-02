using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdatePermission.RequestCommandMapper
{
    public static class UpdatePermissionRequestMapper
    {
        public static UpdatePermissionCommand ToCommand(this UpdatePermissionRequest request, string id)
        {
            return new UpdatePermissionCommand
                (
                    id, 
                    request.name,
                    request.roleId
                    
                );
        }
    }
}
