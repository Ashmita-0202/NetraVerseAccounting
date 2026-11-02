using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.AssignableRoles
{
    public record AssignableRolesResponse
    (
        string roleId = "",
        string permissionId = ""
        );
}
