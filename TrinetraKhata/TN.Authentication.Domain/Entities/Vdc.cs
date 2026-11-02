using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public sealed class Vdc : CustomEntity
    {
        public Vdc(int Id, string vdcNameInEnglish, string vdcNameInNepali, int districtId) : base(Id)
        {
            VdcNameInEnglish = vdcNameInEnglish;
            VdcNameInNepali = vdcNameInNepali;
            DistrictId = districtId;
        }
        public string VdcNameInEnglish { get; set; }
        public string VdcNameInNepali { get; set; }
        public int DistrictId { get; set; }
        public District? Districts { get; set; }
    }
}
