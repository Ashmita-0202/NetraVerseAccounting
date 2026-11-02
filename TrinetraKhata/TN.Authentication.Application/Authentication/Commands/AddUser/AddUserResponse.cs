using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddUser
{
    public record AddUserResponse
   (
        string UserName,
        string Email,
        string Password,
         string? InstitutionId,
        List<string>? companyIds,
        List<string> rolesId

    );
}
