using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddMenu
{
    public record AddMenuResponse
    (
        string Name,
        string IconUrl,
        string TargetUrl,
        string SubModulesId,
        int? Rank,
        bool IsActive
        );
}
