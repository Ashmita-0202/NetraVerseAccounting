using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignMenu.RequestCommandMapper
{
    public static class UpdateAssignMenuRequestMapper
    {
        public static UpdateAssignMenuCommand ToCommand(this UpdateAssignMenuRequest request, string menuId)
        {
            return new UpdateAssignMenuCommand(menuId, request.roleId, request.isActive);
        }

    }
}
