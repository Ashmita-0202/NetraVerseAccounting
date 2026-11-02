using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using TN.Setup.Application.Setup.Queries.VdcById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetVdcByDistrictId
{
    public record class GetVdcByDistrictIdQuery(int Id):IRequest<Result<List<GetVdcByDistrictIdResponse>>>;
}
