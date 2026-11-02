using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Setup.Application.Setup.Queries.MunicipalityById
{
    public record GetMunicipalityByIdResponse(
        int Id,
        string municipalityNameInNepali,
        string municipalityNameInEnglish);
}
