using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Command.DeleteModule
{
    public record DeleteModuleCommand
    (
        string Id
        ): IRequest<Result<bool>>;
}
