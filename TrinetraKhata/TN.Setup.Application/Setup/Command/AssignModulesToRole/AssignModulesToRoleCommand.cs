using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignModulesToRole
{
    public record AssignModulesToRoleCommand
    (
        string roleId,
        IEnumerable<string> modulesId,
        bool isAssigned,
        bool isActive
        ): IRequest<Result<AssignModulesToRoleResponse>>;
}