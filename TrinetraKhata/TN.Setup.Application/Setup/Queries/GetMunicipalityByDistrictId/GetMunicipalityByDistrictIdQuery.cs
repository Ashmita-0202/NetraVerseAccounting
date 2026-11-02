using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetMunicipalityByDistrictId
{
    public record GetMunicipalityByDistrictIdQuery(int Id): IRequest<Result<List<GetMunicipalityByDistrictIdResponse>>>;
   
}
