using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignSubModules.RequestCommandMapper
{
    public static class UpdateAssignSubModulesRequestMapper
    {
        public static UpdateAssignSubModulesCommand ToCommand(this UpdateAssignSubModulesRequest request, string subModulesId)
        {
            return new UpdateAssignSubModulesCommand(subModulesId, request.roleId, request.isActive);
            
        }
    }
}
