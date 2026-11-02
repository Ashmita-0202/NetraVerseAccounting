using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateModules.RequestCommandMapper
{
    public static class UpdateModulesRequestMapper
    {
        public static UpdateModulesCommand ToCommand(this UpdateModulesRequest request, string modulesId)
        {
            return new UpdateModulesCommand(modulesId,request.Name,request.Rank,request.IconUrl, request.TargetUrl,request.IsActive);
        }
    }
}
