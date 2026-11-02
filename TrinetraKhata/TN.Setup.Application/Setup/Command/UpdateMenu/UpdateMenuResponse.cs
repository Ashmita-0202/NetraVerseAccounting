using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;

namespace TN.Setup.Application.Setup.Command.UpdateMenu
{
    public record  UpdateMenuResponse
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
