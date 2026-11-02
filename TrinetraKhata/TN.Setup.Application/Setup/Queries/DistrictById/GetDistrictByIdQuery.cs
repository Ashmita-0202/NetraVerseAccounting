using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Setup.Application.Setup.Queries.ProvinceById;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.DistrictById
{
    public record GetDistrictByIdQuery(
        int Id
        ) : IRequest<Result<GetDistrictByIdResponse>>;
}
