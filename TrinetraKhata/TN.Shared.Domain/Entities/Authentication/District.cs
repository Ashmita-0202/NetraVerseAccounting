using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public sealed class District : CustomEntity
    {
        public District(int Id, string districtNameInNepali, string districtNameInEnglish, int provinceId) : base(Id)
        {
            DistrictNameInNepali = districtNameInNepali;
            DistrictNameInEnglish = districtNameInEnglish;
            ProvinceId = provinceId;
            Municipality = new List<Municipality>();
            Vdcs = new List<Vdc>();

        }
        public string DistrictNameInNepali { get; set; }
        public string DistrictNameInEnglish { get; set; }
        public int ProvinceId { get; set; }
        public Province? Province { get; set; }

        public ICollection<Municipality> Municipality { get; set; }
        public ICollection<Vdc> Vdcs { get; set; }

    }
}
