using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class RoleMenus: Entity
    {
        public RoleMenus(): base(null)
        {
            
        }

        public RoleMenus(
            string id,
            string roleId,
            string menusId,
            bool isActive,
            bool isAssigned
            ): base(id)
        {
            RoleId = roleId;
            MenusId = menusId;
            IsActive = isActive;
            IsAssigned = isAssigned;
            
        }

        public string RoleId { get;set; }
        public IdentityRole Role { get;set; }
        public string MenusId { get;set;}
        public Menu Menu { get;set; }
        public bool IsActive { get;set; }
        public bool IsAssigned { get;set; }
    }
}
