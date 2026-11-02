using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.UpdateCustomerCategory
{
    public record UpdateCustomerCategoryCommand
    (
             string id,
            string name,
            DateTime createdAt,
            bool isEnabled,
            string customerId

    ): IRequest<Result<UpdateCustomerCategoryResponse>>;
}
