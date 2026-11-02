using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Login
{
    public record LoginRequest(string email, string password);
   
}
