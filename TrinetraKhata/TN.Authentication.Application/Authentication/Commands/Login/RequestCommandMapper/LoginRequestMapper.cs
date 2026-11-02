using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Login.RequestCommandMapper
{
    public static class LoginRequestMapper
    {
        public static LoginCommand ToCommand(this LoginRequest request)
        {
            return new LoginCommand(request.email, request.password);
        }
    }
}
