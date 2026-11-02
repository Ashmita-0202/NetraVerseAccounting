using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddModule
{
    public record AddModuleCommand
        (
          string Name,
          string? Rank,
          string? IconUrl,
          string? TargetUrl,
          bool isActive
        ) : IRequest<Result<AddModuleResponse>>;
    
}
