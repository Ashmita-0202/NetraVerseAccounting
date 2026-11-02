using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Command.AddModule
{
    public record AddModuleRequest
    (
      string Name,
      string? Rank,
      string? IconUrl,
      string? TargetUrl,
      bool isActive
        );
}
