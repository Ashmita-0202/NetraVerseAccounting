using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Authentication.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Authentication.Application.Authentication.Queries.FilterUserByDate
{
  public record FilterUserByDateQuery
   (FilterUserDTOs FilterUserDTOs):IRequest<Result<IEnumerable<FilterUserByDateQueryResponse>>>;
}
