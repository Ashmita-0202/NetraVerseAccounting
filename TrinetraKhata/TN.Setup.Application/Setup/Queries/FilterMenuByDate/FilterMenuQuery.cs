using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.FilterMenuByDate
{
    public record  FilterMenuQuery
   (FilterMenuDTOs filterMenuDTOs):IRequest<Result<IEnumerable<FilterMenuQueryResponse>>>;
}
