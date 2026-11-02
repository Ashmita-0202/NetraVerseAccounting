using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles
{
    public record AddPermissionToRolesRequest
    (
        string permissionId,
        List<string> rolesId
        );
}
