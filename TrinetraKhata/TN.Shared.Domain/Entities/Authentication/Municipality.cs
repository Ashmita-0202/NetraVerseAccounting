using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Primitive;

namespace TN.Authentication.Domain.Entities
{
    public sealed class Municipality : CustomEntity
    {
        public Municipality(int Id, string municipalityNameInNepali, string municipalityNameInEnglish, int districtId) : base(Id)
        {
            MunicipalityNameInNepali = municipalityNameInNepali;
            MunicipalityNameInEnglish = municipalityNameInEnglish;
            DistrictId = districtId;

        }
        public string MunicipalityNameInNepali { get; set; }
        public string MunicipalityNameInEnglish { get; set; }
        public int DistrictId { get; set; }
        public District Districts { get; set; }
    }
}
