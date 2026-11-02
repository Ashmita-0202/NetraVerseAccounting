using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Authentication.Application.Authentication.Queries.PermissionById
{
    public record GetPermissionByIdQueryResponse
   (        
            string id,
            string name,
            string roleId

    );
}
