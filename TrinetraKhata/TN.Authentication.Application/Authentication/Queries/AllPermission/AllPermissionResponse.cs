using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.AllPermission
{
    public record AllPermissionResponse
    (
        string id="", 
        string name="",
        string roleId=""
        );
}
