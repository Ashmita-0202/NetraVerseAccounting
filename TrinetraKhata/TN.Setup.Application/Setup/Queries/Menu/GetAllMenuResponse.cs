using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Queries.Menu
{
    public record GetAllMenuResponse
   (
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string role,
            string subModuleId,
            int? rank,
            bool isActive


    )
    {
        public GetAllMenuResponse() : this(string.Empty, string.Empty, null, null, null, string.Empty, 0, false) { }
    }
}
