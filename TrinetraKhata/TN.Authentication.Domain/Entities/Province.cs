using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Setup.Domain.Entities;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public sealed class Province : CustomEntity
    {
        public Province
            (
            int Id,
            string provinceNameInNepali,
            string provinceNameInEnglish
            ) : base(Id)
        {
            ProvinceNameInNepali = provinceNameInNepali;
            ProvinceNameInEnglish = provinceNameInEnglish;
            Districts = new List<District>();
            Organizations = new List<Organization>();

        }

        // Private parameterless constructor for EF compatibility
        private Province() : base(0)
        {
            ProvinceNameInNepali = string.Empty;
            ProvinceNameInEnglish = string.Empty;
            Districts = new List<District>();
            Organizations = new List<Organization>();
        }
        public string ProvinceNameInNepali { get; set; }
        public string ProvinceNameInEnglish { get; set; }
        public ICollection<District> Districts { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }
}
