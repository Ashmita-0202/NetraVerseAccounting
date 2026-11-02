using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class RolePermission : Entity
    {
        public RolePermission(): base(null)
        {
            
        }

        public RolePermission(
            string id,
            string roleId,
            string permissionId
            ): base(id)
        {
            RoleId = roleId;
            PermissionId = permissionId;
            
        }
        public string RoleId { get; set; }
        public virtual IdentityRole Role { get; set; }
        public string PermissionId { get; set; }
        public virtual Permission Permissions { get; set; }

    }
}
