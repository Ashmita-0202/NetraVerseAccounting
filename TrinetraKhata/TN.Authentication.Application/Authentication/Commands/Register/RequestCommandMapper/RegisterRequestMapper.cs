using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.Register.RequestCommandMapper
{
    public static class RegisterRequestMapper
    {
        public static RegisterCommand ToCommand(this RegisterRequest request)
        {
            return new RegisterCommand
                (
                    request.Username,
                    request.Email,
                    request.Password,
                    request.CompanyName,
                    request.Address,
                    request.CompanyShortName,
                    request.ContactNumber,
                    request.ContactPerson,
                    request.PAN




                );
        }
    }
}
