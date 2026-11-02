using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignModulesToRole
{
    public record AssignModulesToRoleResponse
    (
        string roleId,
        List<string> moduleId,
        bool isAssigned,
        bool isActive
        );
}
