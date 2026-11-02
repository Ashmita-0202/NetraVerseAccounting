using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdateUser
{
    public record UpdateUserRequest
    (
        string FirstName,
        string LastName,
        string UserName,
        string Address,
        string Email

    );
}
