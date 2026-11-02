using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.UpdateUser.RequestCommandMapper
{
    public static class UpdateUserRequestMapper
    {
        public static UpdateUserCommand ToCommand(this UpdateUserRequest request, string userId)
        {
            return new UpdateUserCommand
                (
                          userId, 
                          request.FirstName,
                          request.LastName,
                          request.UserName,
                          request.Address,
                          request.Email
            
                );
        }
    }
}
