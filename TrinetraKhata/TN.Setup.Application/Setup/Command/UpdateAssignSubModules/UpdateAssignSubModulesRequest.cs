using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignSubModules
{
    public record UpdateAssignSubModulesRequest
    (
        string roleId,
        bool isActive
        );
}
