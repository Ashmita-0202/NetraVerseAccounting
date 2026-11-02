using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.GetDistrictByProvinceId
{
    public record GetDistrictByProvinceIdResponse
        (
        int Id,
        string districtNameInNepali,
        string districtNameInEnglish,
        int provinceId
        );
    
}
