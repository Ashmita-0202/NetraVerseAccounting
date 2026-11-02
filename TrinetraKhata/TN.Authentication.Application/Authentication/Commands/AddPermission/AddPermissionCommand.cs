using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddPermission
{
    public record AddPermissionCommand
    (
        string name,
        string roleId
        ): IRequest<Result<AddPermissionResponse>>;
}
