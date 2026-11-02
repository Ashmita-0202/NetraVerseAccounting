using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignModules
{
    public record UpdateAssignModulesRequest
    (
        string roleId,
        bool isActive
        );
}
