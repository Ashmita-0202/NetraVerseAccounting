using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.FilterMenuByDate
{
    public record FilterMenuQueryResponse
    (
            string name,
            string targetUrl,
            string iconUrl,
            string role,
            string subModuleId,
            int? rank,
            bool isActive


    );
}
