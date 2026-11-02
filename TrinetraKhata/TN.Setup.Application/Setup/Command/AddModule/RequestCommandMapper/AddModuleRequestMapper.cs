using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddModule.RequestCommandMapper
{
    public static class AddModuleRequestMapper
    {
        public static AddModuleCommand ToCommand(this AddModuleRequest request)
        {
            return new AddModuleCommand(request.Name, request.Rank,request.IconUrl, request.TargetUrl, request.isActive);
        }
    }
}
