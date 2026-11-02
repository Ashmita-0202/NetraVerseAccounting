using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateModules
{
    public record UpdateModulesCommand
    (
        string Id,
        string Name,
        string? Rank,
        string? IconUrl,
        string? TargetUrl,
        bool isActive
        ) : IRequest<Result<UpdateModulesResponse>>;
}
