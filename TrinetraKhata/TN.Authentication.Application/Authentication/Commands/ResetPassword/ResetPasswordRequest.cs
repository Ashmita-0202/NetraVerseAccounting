using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.ResetPassword
{
    public record ResetPasswordRequest
    (
        string email,
        string token,
        string password
        );
}
