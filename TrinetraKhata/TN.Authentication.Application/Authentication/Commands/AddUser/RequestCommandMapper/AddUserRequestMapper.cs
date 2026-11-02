using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Commands.AddUser.RequestCommandMapper
{
   public static class AddUserRequestMapper
    {
        public static AddUserCommand ToCommand(this AddUserRequest addUserRequest)
        {

          return new AddUserCommand
             (
                 addUserRequest.UserName,
                  addUserRequest.Email,
                 addUserRequest.Password,
                 addUserRequest.InstitutionId,         
                 addUserRequest.companyIds,
                 addUserRequest.rolesId

            );
        }
    }
}
