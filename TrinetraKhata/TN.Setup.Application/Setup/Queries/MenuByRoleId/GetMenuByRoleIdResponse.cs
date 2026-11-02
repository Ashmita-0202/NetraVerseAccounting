using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.MenuByRoleId
{
  public record GetMenuByRoleIdResponse
   (
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string subModuleId,
            int? rank,
            bool isActive
  );
}
