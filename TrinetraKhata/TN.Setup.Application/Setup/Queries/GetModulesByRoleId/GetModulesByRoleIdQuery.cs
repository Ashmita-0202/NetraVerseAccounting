using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetModulesByRoleId
{
    public record GetModulesByRoleIdQuery
    (
        string roleId
        ): IRequest<Result<List<GetModulesByRoleIdResponse>>>;
}
