using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.CustomerById
{
    public record GetCustomerByIdQuery(string id) : IRequest<Result<GetCustomerByIdResponse>>;
}
