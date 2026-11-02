using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class SubModules : Entity
    {
        public SubModules(): base(null)
        {
            
        }

        public SubModules(
            string id,
            string name,
            string? iconUrl,
            string? targetUrl,
            string modulesId,
            string rank,
            bool isActive
            ) : base(id)
        {
            Name = name;
            IconUrl = iconUrl;
            ModulesId = modulesId;
            TargetUrl = targetUrl;
            Rank = rank;
            IsActive = isActive;

        }
        public string Name { get; set; }
        public string? IconUrl { get; set; }
        public string? TargetUrl { get; set; }
        public string Rank { get; set; }
        public bool IsActive { get; set; }

        public string ModulesId { get; set; }
        public Modules Modules { get; set; }

        //Navigation Property
        public ICollection<Menu> Menu { get; set; }
        public ICollection<RoleSubModules> RoleSubModules { get; set; }

    }
}
