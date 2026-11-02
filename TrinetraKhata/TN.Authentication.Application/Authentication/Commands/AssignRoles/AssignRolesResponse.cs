using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AssignRoles
{
    public record AssignRolesResponse(
        string UserId,
        List<string> rolesId
        );
    
}
