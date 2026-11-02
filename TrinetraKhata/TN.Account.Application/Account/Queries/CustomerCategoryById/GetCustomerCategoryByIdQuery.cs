using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Queries.CustomerCategoryById
{
   public record GetCustomerCategoryByIdQuery(string id):IRequest<Result<GetCustomerCategoryByIdResponse>>;
    
}
