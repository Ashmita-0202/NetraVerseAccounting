using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Account.Application.Account.Queries.CustomerCategoryById
{
   public record GetCustomerCategoryByIdResponse
   (
            string id,
            string name,
            DateTime createdAt,
            bool isEnabled,
            string customerId
   );
}
