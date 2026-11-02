using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MenuStatusBySubModulesandRolesId
{
    public record MenuStatusBySubModulesAndRolesIdQuery
    (
        string subModulesId,
        string rolesId
        ): IRequest<Result<List<MenuStatusBySubModulesAndRolesIdResponse>>>;
}
