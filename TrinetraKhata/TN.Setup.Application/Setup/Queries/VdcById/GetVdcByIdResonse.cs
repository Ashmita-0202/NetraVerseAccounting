using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.VdcById
{
    public record GetVdcByIdResponse(
        int Id,
        string VdcNameInNepali,
        string VdcNameInEnglish);
}
