using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.OrganizationById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetOrganizationByProvinceId
{
    public record GetOrganizationByProvinceIdQuery(int id) :IRequest<Result<List<GetOrganizationByProvinceIdResponse>>>;  

}
