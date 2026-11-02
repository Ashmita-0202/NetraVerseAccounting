using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.RoleById
{
    public record GetRolesByIdQuery(string Id):IRequest<Result<GetRolesByIdResponse>>;
    
}
