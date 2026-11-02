using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.RoleById
{
    public record GetRolesByIdResponse
        (

          string Id,
          string Name

          )
    {
        public GetRolesByIdResponse(): this(string.Empty, string.Empty)
        {
            
        }
    }
}
