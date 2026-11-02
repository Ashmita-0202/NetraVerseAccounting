using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetModulesByRoleId
{
    public record GetModulesByRoleIdResponse
    (
      string Id,
      string Name,
      string? TargetUrl,
      bool IsActive
        );
}
