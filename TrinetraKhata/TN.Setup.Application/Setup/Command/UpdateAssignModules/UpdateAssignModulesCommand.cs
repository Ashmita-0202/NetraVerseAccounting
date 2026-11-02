using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateAssignModules
{
    public record UpdateAssignModulesCommand
    (
        string modulesId,
        string roleId,
        bool isActive
        ): IRequest<Result<UpdateAssignModulesResponse>>;
}
