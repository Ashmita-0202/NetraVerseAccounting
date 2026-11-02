using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.UpdateUser;

namespace TN.Authentication.Application.Authentication.Commands.UpdateRoles.RequestCommandMapper
{
 public static class UpdateRoleRequestMapper
    {
        public static UpdateRoleCommand ToCommand(this UpdateRoleRequest request, string id)
        {
            return new UpdateRoleCommand
                (
                          id,
                          request.Name
                         
                );
        }
    }
}
