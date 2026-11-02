using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.AddMenu
{
    public record AddMenuCommand
    (
      string Name,
        string IconUrl,
        string TargetUrl,
        string SubModulesId,
        int? Rank,
        bool IsActive

        ) :IRequest<Result<AddMenuResponse>>;
}
