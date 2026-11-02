using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId
{
    public record AssignableRolesByPermissionIdResponse
    (
        string roleId="",
        string permissionId=""
    );
}
