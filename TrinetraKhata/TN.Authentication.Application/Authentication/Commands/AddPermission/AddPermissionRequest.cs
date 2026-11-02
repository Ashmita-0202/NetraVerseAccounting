using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddPermission
{
    public record AddPermissionRequest
    (
        string name,
        string roleId
        );
}
