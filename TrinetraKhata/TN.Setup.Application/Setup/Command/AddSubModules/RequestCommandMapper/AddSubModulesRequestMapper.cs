using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddSubModules.RequestCommandMapper
{
    public static class AddSubModulesRequestMapper
    {
        public static AddSubModulesCommand ToCommand(this AddSubModulesRequest request)
        {
            return new AddSubModulesCommand(request.name, request.iconUrl, request.targetUrl, request.modulesId, request.rank, request.isActive);
        }
    }
}
