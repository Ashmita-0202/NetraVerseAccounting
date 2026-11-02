using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdatePermission
{
    public record UpdatePermissionResponse
    (
             string id,
            string name,
            string roleId
    );
}
