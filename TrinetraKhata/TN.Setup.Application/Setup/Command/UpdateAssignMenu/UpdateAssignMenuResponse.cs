using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignMenu
{
    public record UpdateAssignMenuResponse
       (
        string menuId,
        string roleId,
        bool isActive
        );
}
