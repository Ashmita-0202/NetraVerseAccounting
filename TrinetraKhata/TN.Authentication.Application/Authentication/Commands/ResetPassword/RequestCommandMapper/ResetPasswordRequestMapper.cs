using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.ResetPassword.RequestCommandMapper
{
    public static class ResetPasswordRequestMapper
    {
        public static ResetPasswordCommand ToCommand(this ResetPasswordRequest request)
        {
            return new ResetPasswordCommand(request.email, request.token, request.password);
        }
    }
}
