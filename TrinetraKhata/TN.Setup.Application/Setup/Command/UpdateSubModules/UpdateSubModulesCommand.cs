using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateSubModules
{
    public record UpdateSubModulesCommand
    (

            string id,
            string name,
            string? iconUrl,
            string? targetUrl,
            string moduleId,
            string rank,
            bool isActive

    ) : IRequest<Result<UpdateSubModulesResponse>>;
}
