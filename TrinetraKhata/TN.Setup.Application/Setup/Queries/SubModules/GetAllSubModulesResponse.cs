using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Queries.SubModules
{
    public record GetAllSubModulesResponse
    (
        string id,
        string name,
        string? iconUrl,
        string? targetUrl,
        string? role,
        string modulesId,
        string rank,
        bool isActive
    )
    {
        public GetAllSubModulesResponse() : this(string.Empty, string.Empty, null, null, null, string.Empty, string.Empty, false) { }
    }

}
