using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Command.AddMenu
{
    public record AddMenuRequest
    (
        
        string name,
        string iconUrl,
        string targetUrl,
        string submodulesId,
        int? rank,
        bool isActive
        );
}
