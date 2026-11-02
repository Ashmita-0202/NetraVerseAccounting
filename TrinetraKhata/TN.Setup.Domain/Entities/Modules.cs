using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class Modules : Entity
    {
        public Modules(): base(null)
        {
            
        }
        public Modules(
            string id,
            string name,
            string? rank,
            string? iconUrl,
            string? targetUrl,
            bool isActive
            ) : base(id)
        {
            Name = name;
            Rank = rank;
            IconUrl = iconUrl;
            TargetUrl = targetUrl;
            IsActive = isActive;
            SubModules = new List<SubModules>();
            RoleModules = new List<RoleModule>();

        }
        public string Name { get; set; }
        public string? Rank { get; set; }
        public string? IconUrl { get; set; }
        public string? TargetUrl {  get; set; }
        public bool IsActive { get; set; }

        public ICollection<SubModules> SubModules { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }

    }
}
