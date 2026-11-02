using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateAssignSubModules
{
    public record UpdateAssignSubModulesResponse
    (
        string subModulesId,
        string roleId,
        bool isActive
        );
}
