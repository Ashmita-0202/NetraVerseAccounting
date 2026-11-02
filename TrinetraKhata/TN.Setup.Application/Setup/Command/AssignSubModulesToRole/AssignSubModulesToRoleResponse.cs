using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AssignSubModulesToRole
{
    public record AssignSubModulesToRoleResponse
    (
         string roleId,
        List<string> submodulesId,
        bool isActive,
        bool isAssigned
        );
}
