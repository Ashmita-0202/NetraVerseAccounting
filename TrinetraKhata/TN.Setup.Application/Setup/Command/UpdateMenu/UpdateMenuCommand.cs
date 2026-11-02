using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateMenu
{
    public record UpdateMenuCommand
        (
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string subModulesId,
            int? rank,
            bool isActive


        ) :IRequest<Result<UpdateMenuResponse>>;
    
}
