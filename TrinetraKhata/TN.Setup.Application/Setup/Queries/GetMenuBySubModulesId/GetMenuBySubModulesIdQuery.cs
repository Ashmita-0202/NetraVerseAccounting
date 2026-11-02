using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.GetMenuBySubModulesId
{
    public record GetMenuBySubModulesIdQuery
    (
        string submodulesId
        ): IRequest<Result<List<GetMenuBySubModulesIdResponse>>>;
}
