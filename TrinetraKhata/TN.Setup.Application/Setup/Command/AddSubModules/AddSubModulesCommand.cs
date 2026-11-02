using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddSubModules
{
    public record AddSubModulesCommand
    (
        string name,
        string? iconUrl,
        string? TargetUrl,
        string modulesId,
        string rank,
        bool isActive
        ) : IRequest<Result<AddSubmodulesResponse>>;
}
