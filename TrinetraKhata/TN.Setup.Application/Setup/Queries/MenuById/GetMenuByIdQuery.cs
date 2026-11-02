using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.MenuById
{
    public record GetMenuByIdQuery
    (string id):IRequest<Result<GetMenuByIdResponse>>;
}
