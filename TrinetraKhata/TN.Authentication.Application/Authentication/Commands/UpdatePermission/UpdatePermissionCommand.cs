using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.UpdatePermission
{
  public record UpdatePermissionCommand
  (       
            string id,
            string name,
            string roleId
      
  ) :IRequest<Result<UpdatePermissionResponse>>;
}
