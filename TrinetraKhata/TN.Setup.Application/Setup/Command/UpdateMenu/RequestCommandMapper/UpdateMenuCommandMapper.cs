using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateMenu.RequestCommandMapper
{
    public static class UpdateMenuCommandMapper
    {
        public static UpdateMenuCommand ToCommand(this UpdateMenuRequest request, string menuId)
        {
            return new UpdateMenuCommand
                (
                   menuId,
                   request.name,
                   request.targetUrl,
                   request.iconUrl,
                   request.subModulesId,
                   request.rank,
                   request.isActive
  
                );

        }
    }
}
