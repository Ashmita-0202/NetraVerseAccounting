using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Enum;
using TN.Shared.Domain.Primitive;

namespace TN.Setup.Domain.Entities
{
    public class Menu : Entity
    {
        public Menu() : base(null)
        {
            
        }

        public Menu(
            string id,
            string name,
            string targetUrl,
            string iconUrl,
            string subModulesId,
            int? rank,
            bool isActive

            ) : base(id)
        {
            Name = name;
            TargetUrl = targetUrl;
            IconUrl = iconUrl;
            Rank = rank;
            SubModulesId = subModulesId;
            IsActive = isActive;

        }

        public string Name { get; set; }
        public string TargetUrl { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }

        public int? Rank { get; set; }  // Determines the order of the menu

        public string SubModulesId { get; set; }

        //NavigationProperty
        public SubModules SubModules { get; set; }
        public ICollection<RoleMenus> RoleMenus { get; set;}
    }
}
