using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignSubModulesToRole.RequestCommandMapper
{
    public static class AssignSubModulesRequestMapper
    {
        public static AssignSubModulesToRoleCommand ToCommand(this AssignSubModulesToRoleRequest request)
        {
            return new AssignSubModulesToRoleCommand(request.roleId, request.submodulesId, request.isActive);
        }
    }
}
