using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AssignRoles.RequestCommandMapper
{
    public static class AssignRolesRequestMapper
    {
        public static AssignRolesCommand ToCommand(this AssignRolesRequest request)
        {
            return new AssignRolesCommand(request.userId, request.rolesId);
        }
    }
}
