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
    public record UpdateSubModulesRequest
    (
            string id,
            string name,
            string? iconUrl,
            string? targetUrl,
            string modulesId,
            string rank,
            bool isActive


    );
}
