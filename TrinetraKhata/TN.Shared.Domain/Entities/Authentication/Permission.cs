using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public class Permission : Entity
    {
        public Permission(): base(null)
        {
            
        }
        public Permission(
            string id,
            string name,
            string roleId
            ): base(id)
        {
            Name = name;
            RoleId = roleId;
            RolePermissions = new List<RolePermission>();
        }
        public string Name { get; set; }
        public string RoleId { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
