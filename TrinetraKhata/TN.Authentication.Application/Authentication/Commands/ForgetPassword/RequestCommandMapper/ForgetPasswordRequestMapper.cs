using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.ForgetPassword.RequestCommandMapper
{
    public static class ForgetPasswordRequestMapper
    {
        public static ForgetPasswordCommand ToCommand(this ForgetPasswordRequest request)
        {
            return new ForgetPasswordCommand(request.email);
        }
    }
}
