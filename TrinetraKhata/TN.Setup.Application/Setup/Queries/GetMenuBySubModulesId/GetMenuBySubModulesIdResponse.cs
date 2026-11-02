using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId
{
    public record GetMenuBySubModulesIdResponse
    (
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string subModulesId,
            int? rank,
            bool isActive
        );
}
