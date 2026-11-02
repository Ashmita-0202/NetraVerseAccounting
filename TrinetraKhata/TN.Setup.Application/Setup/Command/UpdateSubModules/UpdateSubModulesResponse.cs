using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Command.UpdateSubModules
{
   public record UpdateSubModulesResponse
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
