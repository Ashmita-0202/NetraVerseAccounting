using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Commands.AddUser
{
   public record AddUserCommand
    (
          string UserName,
        string Email,
        string Password,
         string? InstitutionId,
        List<string>? companyIds,
        List<string> rolesId


     ) : IRequest<Result<AddUserResponse>>;
}
