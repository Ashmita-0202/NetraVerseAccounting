using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignSubModulesToRole
{
    public record AssignSubModulesToRoleCommand
    (
        string roleId,
        IEnumerable<string> submodulesId,
        bool isActive
        ) : IRequest<Result<AssignSubModulesToRoleResponse>>;
}
