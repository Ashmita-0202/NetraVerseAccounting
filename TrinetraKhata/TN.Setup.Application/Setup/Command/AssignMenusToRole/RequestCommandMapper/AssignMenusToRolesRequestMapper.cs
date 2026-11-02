using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignMenusToRole.RequestCommandMapper
{
    public static class AssignMenusToRolesRequestMapper
    {
        public static AssignMenusToRoleCommands ToCommand(this AssignMenusToRoleRequest request)
        {
            return new AssignMenusToRoleCommands(request.roleId, request.menusId, request.isActive);
        }
    }
}
