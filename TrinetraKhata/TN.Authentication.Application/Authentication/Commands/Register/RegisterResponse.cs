using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Register
{
    public record RegisterResponse(
        string Username,
        string Email,
        string Password,
        string CompanyName,
        string Address,
        string CompanyShortName,
        string ContactNumber,
        string ContactPerson,
        string PAN

        );
   
}
