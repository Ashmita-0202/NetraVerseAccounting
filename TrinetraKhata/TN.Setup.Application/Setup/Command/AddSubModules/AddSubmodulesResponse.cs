using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddSubModules
{
    public record AddSubmodulesResponse
    (
        string name,
        string? iconUrl,
        string? TargetUrl,
        string? rank,
        bool isActive
        );
}
