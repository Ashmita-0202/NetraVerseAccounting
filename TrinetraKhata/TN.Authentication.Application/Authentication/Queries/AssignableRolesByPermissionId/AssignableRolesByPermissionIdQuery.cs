using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace TN.Authentication.Application.Authentication.Queries.AssignableRolesByPermissionId
{
    public record class AssignableRolesByPermissionIdQuery
    (
        string permissionId
        ) : IRequest<Result<List<AssignableRolesByPermissionIdResponse>>>;
}
