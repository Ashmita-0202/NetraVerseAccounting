using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetSubModulesByModulesId
{
    public record GetSubModulesByModulesIdResponse
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
