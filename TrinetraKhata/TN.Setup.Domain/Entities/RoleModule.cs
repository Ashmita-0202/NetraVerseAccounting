using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class RoleModule : Entity
    {

        public RoleModule() : base(null)
        {
            
        }

        public RoleModule(
            string id,
            string roleId,
            string moduleId,
            bool isAssigned,
            bool isActive

            ) : base(id)
        {
            RoleId = roleId;
            ModuleId = moduleId;
            IsActive = isActive;
            IsAssigned = isAssigned;

        }

        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        public string ModuleId { get; set; }
        public Modules Modules { get; set; }
        public bool IsActive { get; set; }
        public bool IsAssigned { get; set; } 
    }
}
