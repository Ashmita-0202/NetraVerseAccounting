using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AssignMenusToRole
{
    public record AssignMenusToRoleCommands
    (
         string roleId,
        IEnumerable<string> menusId,
        bool isActive
        ) : IRequest<Result<AssignMenuToRolesResponse>>;
}
