using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Application.Setup.Command.AddModule;

namespace TN.Setup.Application.Setup.Command.AssignModulesToRole.RequestCommandMapper
{
    public static class AssignModulesRequestMapper
    {
        public static AssignModulesToRoleCommand ToCommand(this AssignModulesToRoleRequest request)
        {
            return new AssignModulesToRoleCommand(request.roleId,request.modulesId,request.isAssigned, request.isActive);
        }
    }
}
