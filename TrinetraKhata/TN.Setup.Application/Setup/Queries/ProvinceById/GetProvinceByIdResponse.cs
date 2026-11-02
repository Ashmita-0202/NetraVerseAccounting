using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.ProvinceById
{
    public record GetProvinceByIdResponse(
        int Id,
        string provinceNameInNepali,
        string provinceNameInEnglish
        );
    
}
