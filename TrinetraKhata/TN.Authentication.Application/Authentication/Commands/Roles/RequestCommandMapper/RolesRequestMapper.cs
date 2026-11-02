using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Application.Authentication.Commands.AssignRoles;

namespace TN.Authentication.Application.Authentication.Commands.Roles.RequestCommandMapper
{
    public static class RolesRequestMapper
    {
        public static RoleCommand ToCommand(this RolesRequest request)
        {
            return new RoleCommand(request.Name);
        }
    }
}
