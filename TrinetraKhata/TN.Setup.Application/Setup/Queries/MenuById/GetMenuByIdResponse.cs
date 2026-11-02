using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Queries.MenuById
{
    public record GetMenuByIdResponse
    (
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string role,
            string subModulesId,
            int? rank,
            bool isActive
    )
    {
        public GetMenuByIdResponse() : this(string.Empty, string.Empty, null, null, null, string.Empty, 0, false) { }
    }

}

