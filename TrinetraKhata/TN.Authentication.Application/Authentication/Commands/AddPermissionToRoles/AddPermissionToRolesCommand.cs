using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddPermissionToRoles
{
    public record AddPermissionToRolesCommand
    (
        string permissionId,
        List<string> rolesId
        ): IRequest<Result<AddPermissionToRolesResponse>>;
}
