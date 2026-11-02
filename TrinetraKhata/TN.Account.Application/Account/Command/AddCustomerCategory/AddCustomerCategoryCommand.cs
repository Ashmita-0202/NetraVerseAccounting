using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace TN.Account.Application.Account.Command.AddCustomerCategory
{
    public record AddCustomerCategoryCommand
    (
            string name,
            DateTime createdAt,
            bool isEnabled,
            string customerId

    ):IRequest<Result<AddCustomerCategoryResponse>>;
}
