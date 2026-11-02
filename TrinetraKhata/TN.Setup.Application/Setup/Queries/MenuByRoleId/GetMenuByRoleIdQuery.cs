using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MenuByRoleId
{
   public record GetMenuByRoleIdQuery
    (string roleId):IRequest<Result<List<GetMenuByRoleIdResponse>>>;
}
