using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.ForgetPassword
{
    public record ForgetPasswordRequest
        (
        string email
        );
    
}
