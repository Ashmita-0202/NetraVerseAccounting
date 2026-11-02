using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId
{
    public record class GetMunicipalityByDistrictIdResponse(
        int Id,
        string MunicipalityNameinNepali,
        string MunicipalityNameinEnglish,
        int DistrictId);
}
