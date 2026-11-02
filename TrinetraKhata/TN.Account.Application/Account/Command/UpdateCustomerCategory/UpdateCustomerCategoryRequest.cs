using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Command.UpdateCustomerCategory
{
    public record UpdateCustomerCategoryRequest
    (
            string name,
            DateTime createdAt,
            bool isEnabled,
            string customerId
    );
}
