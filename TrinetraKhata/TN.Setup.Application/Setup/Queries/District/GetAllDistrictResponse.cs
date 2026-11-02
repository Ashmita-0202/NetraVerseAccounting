using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.District
{
    public record GetAllDistrictResponse
       (
       int Id,
       string districtNameInNepali,
       string districtNameInEnglish
       );
}
