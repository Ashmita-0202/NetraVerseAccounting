using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignMenusToRole
{
    public record AssignMenusToRoleRequest
    (
        string roleId,
        IEnumerable<string> menusId,
        bool isActive
        );
}
