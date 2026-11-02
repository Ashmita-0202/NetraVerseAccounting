using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.PermissionById
{
  public record GetPermissionByIdQuery
   (string id):IRequest<Result<GetPermissionByIdQueryResponse>>;
}
