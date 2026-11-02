using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Queries.GetSubModulesById
{
    public record GetSubModulesByIdResponse
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
        public GetSubModulesByIdResponse() : this(string.Empty, string.Empty, null, null, null, string.Empty, string.Empty, false) { }
    }

}
