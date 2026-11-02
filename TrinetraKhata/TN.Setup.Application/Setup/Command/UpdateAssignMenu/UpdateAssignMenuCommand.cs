using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.UpdateAssignMenu
{
    public record UpdateAssignMenuCommand
   (
        string menuId,
        string roleId,
        bool isActive
        ) : IRequest<Result<UpdateAssignMenuResponse>>;
}
