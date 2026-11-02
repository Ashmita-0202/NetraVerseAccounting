using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetSubModulesByRoleId
{
  public record GetSubModulesByRoleIdResponse
    (       
            string id,
            string name,
            string? iconUrl,
            string? targetUrl,
            string modulesId,
            string rank,
            bool isActive
      );
}
