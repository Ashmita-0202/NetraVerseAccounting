using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignModulesToRole
{
    public record AssignModulesToRoleRequest
        (
        string roleId,
        IEnumerable<string> modulesId,
        bool isAssigned,
        bool isActive
        );
   
}
