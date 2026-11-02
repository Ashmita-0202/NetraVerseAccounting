using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddMenu.RequestCommandMapper
{
    public static class AddMenuRequestMapper
    {
        public static AddMenuCommand ToCommand(this AddMenuRequest request)
        {
            return new AddMenuCommand(request.name, request.iconUrl, request.targetUrl, request.submodulesId,request.rank,request.isActive);
        }
    }
}
