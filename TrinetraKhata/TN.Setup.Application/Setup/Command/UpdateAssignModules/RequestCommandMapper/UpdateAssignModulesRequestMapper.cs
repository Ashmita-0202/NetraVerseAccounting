using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignModules.RequestCommandMapper
{
    public static class UpdateAssignModulesRequestMapper
    {
        public static UpdateAssignModulesCommand ToCommand(this UpdateAssignModulesRequest request, string modulesId)
        {
            return new UpdateAssignModulesCommand(modulesId, request.roleId, request.isActive);
        }
    }
}
