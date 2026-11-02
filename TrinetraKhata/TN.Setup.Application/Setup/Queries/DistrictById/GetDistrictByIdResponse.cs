using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.DistrictById
{
    public record GetDistrictByIdResponse(
        string DistrictNameInEnglish,
        string DistrictNameInNepali,
        int Id,
        int ProvinceId
        );
}
