using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.NavigationByUser
{
    public record SubModulesResponse(
        string modulesId,
      string subModulesId,
      string name,
      string? iconUrl,
      string? targetUrl,
      string? role,
      string? rank,
      bool isActive
        );

}
