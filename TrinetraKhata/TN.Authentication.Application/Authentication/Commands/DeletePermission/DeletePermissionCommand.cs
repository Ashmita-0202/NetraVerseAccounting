using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.DeletePermission
{
   public record DeletePermissionCommand
    (string id):IRequest<Result<bool>>;
}
