using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AssignRoles
{
    public record AssignRolesRequest
    (
        string userId,
        List<string> rolesId
        );
}
