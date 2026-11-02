using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.UpdateModules
{
    public record UpdateModulesRequest
    (
      string Name,
      string? Rank,
      string? IconUrl,
      string? TargetUrl,
      bool IsActive
        );
}
