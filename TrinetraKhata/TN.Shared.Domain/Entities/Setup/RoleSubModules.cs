using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class RoleSubModules: Entity
    {
        public RoleSubModules(): base(null)
        {
            
        }

        public RoleSubModules(
            string id,
            string roleId,
            string subModulesId,
            bool isAssigned,
            bool isActive
            ): base(id)
        {
            RoleId = roleId;
            SubModulesId = subModulesId;
            IsActive = isActive;
            IsAssigned = isAssigned;
            
        }
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public string SubModulesId { get; set;}
        public SubModules SubModules { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; }
    }
}
